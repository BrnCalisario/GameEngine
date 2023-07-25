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

    public static void Load()
    {
        CookerImage = Image.FromFile(AssetsPath + "\\player_3x.png");
        TileImage = Image.FromFile(AssetsPath + "\\tileShadows.png");
        BenchImage = Image.FromFile(AssetsPath + "\\bench3x.png");
        TrashImage = Image.FromFile(AssetsPath + "\\trash3x.png");
        FoodImage = Image.FromFile(AssetsPath + "\\food3x.png");
        PanImage = Image.FromFile(AssetsPath + "\\panela3x.png");
        PlateImage = Image.FromFile(AssetsPath + "\\plate3x.png");
        CuttingBoardImage = Image.FromFile(AssetsPath + "\\tabua3x.png");
    }
}
