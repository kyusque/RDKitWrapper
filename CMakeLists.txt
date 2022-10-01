cmake_minimum_required(VERSION 3.22)
project(RDKitWrapper)
set(RDKit_DIR ${RDKit_ROOT_DIR})
set(LIB_PREFIX ${LIB_PREFIX})
set(ADD_LINK_DIR ${ADD_LINK_DIR})

add_library(RDKitWrapper SHARED ../rdkit_wrapper.cpp ../rdkit_wrapper.def)
find_package(Freetype REQUIRED)
find_package(RDKit REQUIRED)
target_include_directories(RDKitWrapper
    PRIVATE
    ${RDKit_INCLUDE_DIRS}
    ${Boost_INCLUDE_DIRS}
)
target_link_directories(RDKitWrapper
    PRIVATE
    ${ADD_LINK_DIR}
)
target_link_libraries(RDKitWrapper
    ${LIB_PREFIX}Abbreviations
    ${LIB_PREFIX}Alignment
    ${LIB_PREFIX}CIPLabeler
    ${LIB_PREFIX}Catalogs
    ${LIB_PREFIX}ChemReactions
    ${LIB_PREFIX}ChemTransforms
    ${LIB_PREFIX}ChemicalFeatures
    ${LIB_PREFIX}DataStructs
    ${LIB_PREFIX}Depictor
    ${LIB_PREFIX}Deprotect
    ${LIB_PREFIX}Descriptors
    ${LIB_PREFIX}DistGeomHelpers
    ${LIB_PREFIX}DistGeometry
    ${LIB_PREFIX}EigenSolvers
    ${LIB_PREFIX}FMCS
    ${LIB_PREFIX}FileParsers
    ${LIB_PREFIX}FilterCatalog
    ${LIB_PREFIX}Fingerprints
    ${LIB_PREFIX}ForceField
    ${LIB_PREFIX}ForceFieldHelpers
    ${LIB_PREFIX}FragCatalog
    ${LIB_PREFIX}GenericGroups
    ${LIB_PREFIX}GraphMol
    ${LIB_PREFIX}InfoTheory
    ${LIB_PREFIX}MMPA
    ${LIB_PREFIX}MolAlign
    ${LIB_PREFIX}MolCatalog
    ${LIB_PREFIX}MolChemicalFeatures
    ${LIB_PREFIX}MolDraw2D
    ${LIB_PREFIX}MolEnumerator
    ${LIB_PREFIX}MolHash
    ${LIB_PREFIX}MolInterchange
    ${LIB_PREFIX}MolStandardize
    ${LIB_PREFIX}MolTransforms
    ${LIB_PREFIX}O3AAlign
    ${LIB_PREFIX}Optimizer
    ${LIB_PREFIX}PartialCharges
    ${LIB_PREFIX}RDGeneral
    ${LIB_PREFIX}RDGeometryLib
    ${LIB_PREFIX}RDStreams
    ${LIB_PREFIX}RGroupDecomposition
    ${LIB_PREFIX}ReducedGraphs
    ${LIB_PREFIX}RingDecomposerLib
    ${LIB_PREFIX}ScaffoldNetwork
    ${LIB_PREFIX}ShapeHelpers
    ${LIB_PREFIX}SimDivPickers
    ${LIB_PREFIX}SmilesParse
    ${LIB_PREFIX}Subgraphs
    ${LIB_PREFIX}SubstructLibrary
    ${LIB_PREFIX}SubstructMatch
    ${LIB_PREFIX}TautomerQuery
    ${LIB_PREFIX}Trajectory
    ${LIB_PREFIX}coordgen
    ${LIB_PREFIX}ga
    ${LIB_PREFIX}hc
    ${FREETYPE_LIBRARIES}
)
