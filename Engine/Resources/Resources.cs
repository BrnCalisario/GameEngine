using System.Drawing;

namespace Engine.Resource;

using static ProjectPaths;

public static class Resources
{
    public static Image CookerImage { get; private set;}

    public static Image TileImage  { get; private set;}

    public static Image BenchImage { get; private set; }

    public static Image TrashImage { get; private set; }

    public static Image FoodImage { get; private set; }

    public static Image PanImage { get; private set; }

    public static Image PlateImage { get; private set; }

    public static Image CuttingBoardImage { get; private set; }
    public static Image CheckOutImage { get; private set; }

    public static void Load()
    {
        CookerImage = Image.FromFile(AssetsPath + "\\player\\player_3x.png");
        TileImage = Image.FromFile(AssetsPath + "\\tiles\\tileShadows.png");
        BenchImage = Image.FromFile(AssetsPath + "\\benchs\\bench3x.png");
        TrashImage = Image.FromFile(AssetsPath + "\\trash\\trash3x.png");
        FoodImage = Image.FromFile(AssetsPath + "\\foods\\foodSliced3x.png");
        PlateImage = Image.FromFile(AssetsPath + "\\plates\\plate3x.png");
        CuttingBoardImage = Image.FromFile(AssetsPath + "\\tabua3x.png");
        CheckOutImage = Image.FromFile(AssetsPath + "\\esteira3x.png");
        PanImage = Image.FromFile(AssetsPath + "\\pans\\panela3x.png");
    }
}
