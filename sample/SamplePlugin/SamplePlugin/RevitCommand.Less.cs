namespace SamplePlugin {
    public partial class RevitCommand {
#if REVIT_2016_OR_LESS
        public int RevitVersionOrLess2016 => 2016;
#endif

#if REVIT_2017_OR_LESS
        public int RevitVersionOrLess2017 => 2017;
#endif

#if REVIT_2018_OR_LESS
        public int RevitVersionOrLess2018 => 2018;
#endif

#if REVIT_2019_OR_LESS
        public int RevitVersionOrLess2019 => 2019;
#endif

#if REVIT_2020_OR_LESS
        public int RevitVersionOrLess2020 => 2020;
#endif

#if REVIT_2021_OR_LESS
        public int RevitVersionOrLess2021 => 2021;
#endif

#if REVIT_2022_OR_LESS
        public int RevitVersionOrLess2022 => 2022;
#endif

#if REVIT_2023_OR_LESS
        public int RevitVersionOrLess2023 => 2023;
#endif

#if REVIT_2024_OR_LESS
        public int RevitVersionOrLess2024 => 2024;
#endif
    }
}