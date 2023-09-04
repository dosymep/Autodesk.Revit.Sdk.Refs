namespace SamplePlugin {
    public partial class RevitCommand {
#if REVIT_2016_OR_GREATER
        public int RevitVersionOrGreater2016 => 2016;
#endif

#if REVIT_2017_OR_GREATER
        public int RevitVersionOrGreater2017 => 2017;
#endif

#if REVIT_2018_OR_GREATER
        public int RevitVersionOrGreater2018 => 2018;
#endif

#if REVIT_2019_OR_GREATER
        public int RevitVersionOrGreater2019 => 2019;
#endif

#if REVIT_2020_OR_GREATER
        public int RevitVersionOrGreater2020 => 2020;
#endif

#if REVIT_2021_OR_GREATER
        public int RevitVersionOrGreater2021 => 2021;
#endif

#if REVIT_2022_OR_GREATER
        public int RevitVersionOrGreater2022 => 2022;
#endif

#if REVIT_2023_OR_GREATER
        public int RevitVersionOrGreater2023 => 2023;
#endif

#if REVIT_2024_OR_GREATER
        public int RevitVersionOrGreater2024 => 2024;
#endif
    }
}