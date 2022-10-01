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
var exactNumAtoms = new int[1]{-1};
var atomicNums = new int[maxNumAtoms];
var atomCharges = new int[maxNumAtoms];
var positons = new double[3 * maxNumAtoms];
var numBonds = new int[1]{0};
var bondConnections = new int[maxNumAtoms * maxNumAtoms];
var bondOrders = new double[maxNumAtoms * maxNumAtoms];
OptFromSmiles(smiles, atomicNums, atomCharges, positons, bondConnections, bondOrders, exactNumAtoms, numBonds);
Console.WriteLine(exactNumAtoms[0]);
if (exactNumAtoms[0] > 0)
{
    for(var i = 0; i < exactNumAtoms[0]; i++)
    {
        Console.WriteLine("" + atomicNums[i] + ": " + atomCharges[i] + ": " + positons[3 * i + 0] + ", " + positons[3 * i + 1] + ", " + positons[3 * i + 2]);
    }
}
if (numBonds[0] > 0)
{   
    Console.WriteLine(numBonds[0]);
    for(var i = 0; i < numBonds[0]; i++)
    {
        Console.WriteLine("" + bondOrders[i] + ": " + bondConnections[2 * i + 0] + ", " + bondConnections[2 * i + 1]);
    }
}
Console.WriteLine("[Test End] OptFromSmiles");

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