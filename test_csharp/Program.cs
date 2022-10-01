using System.Runtime.InteropServices;
using System.Text;

Console.WriteLine("[Test Start] DrawSVG");
var smiles = "c1ccccc1";
Int32 bufsize = 1000000;
var stringBuilder = new StringBuilder(bufsize);
DrawSVG(smiles, stringBuilder, bufsize);
Console.WriteLine(stringBuilder.ToString());
Console.WriteLine("[Test End] DrawSVG");


Console.WriteLine("[Test Start] OptFromSmiles");
var maxNumAtoms = 20;
var exactNumAtoms = new int[1] { -1 };
var atomicNums = new int[maxNumAtoms];
var atomCharges = new int[maxNumAtoms];
var positons = new double[3 * maxNumAtoms];
var numBonds = new int[1] { 0 };
var bondConnections = new int[maxNumAtoms * maxNumAtoms];
var bondOrders = new double[maxNumAtoms * maxNumAtoms];
OptFromSmiles(smiles, atomicNums, atomCharges, positons, bondConnections, bondOrders, exactNumAtoms, numBonds);
Console.WriteLine(exactNumAtoms[0]);
if (exactNumAtoms[0] > 0)
{
    for (var i = 0; i < exactNumAtoms[0]; i++)
    {
        Console.WriteLine("" + atomicNums[i] + ": " + atomCharges[i] + ": " + positons[3 * i + 0] + ", " + positons[3 * i + 1] + ", " + positons[3 * i + 2]);
    }
}
if (numBonds[0] > 0)
{
    Console.WriteLine(numBonds[0]);
    for (var i = 0; i < numBonds[0]; i++)
    {
        Console.WriteLine("" + bondOrders[i] + ": " + bondConnections[2 * i + 0] + ", " + bondConnections[2 * i + 1]);
    }
}
Console.WriteLine("[Test End] OptFromSmiles");

Console.WriteLine("[Test Start] OptMolecule");
positons = new double[3 * exactNumAtoms[0]];
OptMolecule(exactNumAtoms[0], atomicNums, atomCharges, numBonds[0], bondConnections, bondOrders, positons);
Console.WriteLine(exactNumAtoms[0]);
if (exactNumAtoms[0] > 0)
{
    for (var i = 0; i < exactNumAtoms[0]; i++)
    {
        Console.WriteLine("" + atomicNums[i] + ": " + atomCharges[i] + ": " + positons[3 * i + 0] + ", " + positons[3 * i + 1] + ", " + positons[3 * i + 2]);
    }
}
if (numBonds[0] > 0)
{
    Console.WriteLine(numBonds[0]);
    for (var i = 0; i < numBonds[0]; i++)
    {
        Console.WriteLine("" + bondOrders[i] + ": " + bondConnections[2 * i + 0] + ", " + bondConnections[2 * i + 1]);
    }
}
Console.WriteLine("[Test End] OptMolecule");

Console.WriteLine("[Test Start] ReadSdf");
maxNumAtoms = 1000000;
var numMolecules = new int[1] { 1000 };
atomicNums = new int[maxNumAtoms];
atomCharges = new int[maxNumAtoms];
positons = new double[3 * maxNumAtoms];
var numAtoms = new int[numMolecules[0]];
numBonds = new int[numMolecules[0]];
bondConnections = new int[maxNumAtoms];
bondOrders = new double[maxNumAtoms];
ReadSdf("withHs.sdf", numMolecules, numAtoms, atomicNums, atomCharges, numBonds, bondConnections, bondOrders, positons);
var atomOffset = 0;
var bondOffset = 0;
for (var j = 0; j < numMolecules[0]; j++)
{
    if (numAtoms[j] > 0)
    {
        Console.WriteLine("NumAtoms: " + numAtoms[j]);
        for (var i = 0; i < numAtoms[j]; i++)
        {
            Console.WriteLine("" + atomicNums[atomOffset + i] + ": " + atomCharges[atomOffset + i] + ": " + positons[3 * (atomOffset + i) + 0] + ", " + positons[3 * (atomOffset + i) + 1] + ", " + positons[3 * (atomOffset + i) + 2]);
        }
        atomOffset += numAtoms[j];
    }
    if (numBonds[j] > 0)
    {
        Console.WriteLine("NumBonds: " + numBonds[j]);
        for (var i = 0; i < numBonds[j]; i++)
        {
            Console.WriteLine("" + bondOrders[bondOffset + i] + ": " + bondConnections[2 * (bondOffset + i) + 0] + ", " + bondConnections[2 * (bondOffset + i) + 1]);
        }
        bondOffset += numBonds[j];
    }
}
Console.WriteLine("[Test End] ReadSdf");

[DllImport("RDKitWrapper")]
extern static int DrawSVG(string smiles, [Out] StringBuilder buf, Int32 bufsize);

[DllImport("RDKitWrapper")]
extern static int OptFromSmiles(
    string smiles,
    [Out] int[] atomicNums,
    [Out] int[] atomCharges,
    [Out] double[] positions,
    [Out] int[] bondConnections,
    [Out] double[] bondOrders,
    [Out] int[] numAtoms,
    [Out] int[] numBonds
    );

[DllImport("RDKitWrapper")]
extern static int OptMolecule(
    [In] int numAtoms,
    [In] int[] atomicNums,
    [In] int[] atomCharges,
    [In] int numBonds,
    [In] int[] bondConnections,
    [In] double[] bondOrders,
    [Out] double[] positions
    );

[DllImport("RDKitWrapper")]
extern static int ReadSdf(
    [In] string path,
    [Out] int[] numMolecules,
    [Out] int[] numAtoms,
    [Out] int[] atomicNums,
    [Out] int[] atomCharges,
    [Out] int[] numBonds,
    [Out] int[] bondConnections,
    [Out] double[] bondOrders,
    [Out] double[] positions
    );