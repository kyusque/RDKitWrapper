#include <iostream>
#include <string.h>
#include <GraphMol/SmilesParse/SmilesParse.h>
#include <GraphMol/MolOps.h>
#include <GraphMol/MolDraw2D/MolDraw2DSVG.h>
#include <GraphMol/Depictor/RDDepictor.h>
#include <GraphMol/FileParsers/MolFileStereochem.h>
#include <ForceField/ForceField.h>
#include <ForceField/MMFF/Params.h>
#include <ForceField/MMFF/DistanceConstraint.h>
#include <ForceField/MMFF/AngleConstraint.h>
#include <ForceField/MMFF/TorsionConstraint.h>
#include <ForceField/MMFF/PositionConstraint.h>
#include <GraphMol/ForceFieldHelpers/MMFF/AtomTyper.h>
#include <GraphMol/ForceFieldHelpers/MMFF/Builder.h>
#include <GraphMol/DistGeomHelpers/Embedder.h>
#include <GraphMol/MolTransforms/MolTransforms.h>

extern "C"
{
    int DrawSVG(char *smiles, char *buf, size_t bufsize);
    int OptFromSmiles(char *smiles, int *atomicNumbers, int *atomCharges, double *positions, int numAtoms);
}

int DrawSVG(char *smiles, char *buf, size_t bufsize)
{
    try
    {
        auto mol = RDKit::SmilesToMol(smiles);
        if (mol == nullptr)
        {
            throw std::runtime_error("failed to parse smiles");
        }
        RDKit::MolOps::addHs(*mol);
        auto drawer = new RDKit::MolDraw2DSVG(300, 300, -1, -1, true);
        drawer->drawMolecule(*mol);
        std::string text = drawer->getDrawingText() + "</svg>\n";
        if (bufsize < text.length())
        {
            delete drawer;
            delete mol;
            std::string msg = "bufsize is not enough. ";
            msg += std::to_string(bufsize);
            msg += " < ";
            msg += std::to_string(text.length());
            throw std::runtime_error(msg);
        }
        for (size_t i = 0; i < text.length(); i++)
        {
            buf[i] = text[i];
        }
        buf[text.length()] = '\0';
        delete drawer;
        delete mol;
    }
    catch (std::runtime_error e)
    {
        std::cerr << "RDKitWrapper runtime_error: " << e.what() << std::endl;
        return -1;
    }
    catch (...)
    {
        std::cerr << "RDKitWrapper unknown exception" << std::endl;
        return -1;
    }
    return 0;
}

int OptFromSmiles(char *smiles, int *atomicNumbers, int *atomCharges, double *positions, int *bondConnections, double *bondOrders, int *numAtoms, int *numBonds)
{
    int max_minimize = 5;
    try
    {
        auto mol = RDKit::SmilesToMol(smiles);
        if (mol == nullptr)
        {
            throw std::runtime_error("failed to parse smiles");
        }
        auto embededs = RDKit::DGeomHelpers::EmbedMolecule(*mol);
        RDKit::MolOps::addHs(*mol, false, true);
        if (embededs < 0)
        {
            throw std::runtime_error("failed to embed molecules");
        };
        RDKit::MMFF::sanitizeMMFFMol(*mol);
        auto *prop = new RDKit::MMFF::MMFFMolProperties(*mol);
        auto *field = RDKit::MMFF::constructForceField(*mol, prop);
        field->initialize();
        field->minimize(max_minimize);
        auto conf = mol->getConformer();
        RDGeom::Point3D pos;
        numAtoms[0] = (int)mol->getNumAtoms();
        for (int i = 0; i < numAtoms[0]; i++)
        {
            atomicNumbers[i] = mol->getAtomWithIdx(i)->getAtomicNum();
            atomCharges[i] = mol->getAtomWithIdx(i)->getFormalCharge();
            pos = conf.getAtomPos(i);
            positions[3 * i + 0] = pos.x;
            positions[3 * i + 1] = pos.y;
            positions[3 * i + 2] = pos.z;
        }
        RDKit::MolOps::Kekulize(*mol);
        numBonds[0] = (int)mol->getNumBonds();
        for (int i = 0; i < numBonds[0]; i++) {
            bondConnections[2 * i + 0] = (int)mol->getBondWithIdx(i)->getBeginAtomIdx();
            bondConnections[2 * i + 1] = (int)mol->getBondWithIdx(i)->getEndAtomIdx();
            bondOrders[i] = (double)mol->getBondWithIdx(i)->getBondTypeAsDouble();
        }
        delete field;
        delete prop;
        delete mol;
    }
    catch (std::runtime_error e)
    {
        std::cerr << "RDKitWrapper runtime_error: " << e.what() << std::endl;
        return -1;
    }
    catch (...)
    {
        std::cerr << "RDKitWrapper unknown exception" << std::endl;
        return -1;
    }
    return 0;
}


int OptMolecule(int numAtoms, int* atomicNumbers, int* atomCharges, int numBonds, int* bondConnections, double* bondOrders,  double* positions)
{
    int max_minimize = 5;
    try
    {
        auto mol = new RDKit::RWMol();
        for (int i = 0; i < numAtoms; i++) {
            auto atom = new RDKit::Atom(atomicNumbers[i]);
            atom->setFormalCharge(atomCharges[i]);
            mol->addAtom(atom);
        }
        for (int i = 0; i < numBonds; i++) {
            auto bond = new RDKit::Bond();
            bond->setBeginAtomIdx(bondConnections[2 * i + 0]);
            bond->setEndAtomIdx(bondConnections[2 * i + 1]);
            switch ((int)bondOrders[i])
            {
                case 1:
                    bond->setBondType(RDKit::Bond::SINGLE);
                    break;
                case 2:
                    bond->setBondType(RDKit::Bond::DOUBLE);
                    break;
                case 3:
                    bond->setBondType(RDKit::Bond::TRIPLE);
                    break;
                default:
                    bond->setBondType(RDKit::Bond::SINGLE);
                    break;
            }
            mol->addBond(bond);
        }
        RDKit::MolOps::KekulizeIfPossible(*mol); // for initialize ring information
        auto embededs = RDKit::DGeomHelpers::EmbedMolecule(*mol);
        RDKit::MMFF::sanitizeMMFFMol(*mol);
        auto* prop = new RDKit::MMFF::MMFFMolProperties(*mol);
        auto* field = RDKit::MMFF::constructForceField(*mol, prop);
        field->initialize();
        field->minimize(max_minimize);
        auto conf = mol->getConformer();
        RDGeom::Point3D pos;
        for (int i = 0; i < numAtoms; i++)
        {
            atomicNumbers[i] = mol->getAtomWithIdx(i)->getAtomicNum();
            atomCharges[i] = mol->getAtomWithIdx(i)->getFormalCharge();
            pos = conf.getAtomPos(i);
            positions[3 * i + 0] = pos.x;
            positions[3 * i + 1] = pos.y;
            positions[3 * i + 2] = pos.z;
        }
        RDKit::MolOps::Kekulize(*mol);
        for (int i = 0; i < numBonds; i++) {
            bondConnections[2 * i + 0] = (int)mol->getBondWithIdx(i)->getBeginAtomIdx();
            bondConnections[2 * i + 1] = (int)mol->getBondWithIdx(i)->getEndAtomIdx();
            bondOrders[i] = (double)mol->getBondWithIdx(i)->getBondTypeAsDouble();
        }
        delete field;
        delete prop;
        delete mol;
    }
    catch (std::runtime_error e)
    {
        std::cerr << "RDKitWrapper runtime_error: " << e.what() << std::endl;
        return -1;
    }
    catch (...)
    {
        std::cerr << "RDKitWrapper unknown exception" << std::endl;
        return -1;
    }
    return 0;
}
