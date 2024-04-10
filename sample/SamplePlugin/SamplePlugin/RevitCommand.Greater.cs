namespace SamplePlugin {
    public partial class RevitCommand {
#if REVIT2016_OR_GREATER
        public int RevitVersionOrGreater2016 => 2016;
#endif

#if REVIT2017_OR_GREATER
        public int RevitVersionOrGreater2017 => 2017;
#endif

#if REVIT2018_OR_GREATER
        public int RevitVersionOrGreater2018 => 2018;
#endif

#if REVIT2019_OR_GREATER
        public int RevitVersionOrGreater2019 => 2019;
#endif

#if REVIT2020_OR_GREATER
        public int RevitVersionOrGreater2020 => 2020;
#endif

#if REVIT2021_OR_GREATER
        public int RevitVersionOrGreater2021 => 2021;
#endif

#if REVIT2022_OR_GREATER
        public int RevitVersionOrGreater2022 => 2022;
#endif

#if REVIT2023_OR_GREATER
        public int RevitVersionOrGreater2023 => 2023;
#endif

#if REVIT2024_OR_GREATER
        public int RevitVersionOrGreater2024 => 2024;
#endif
        
#if REVIT2025_OR_GREATER
        public int RevitVersionOrGreater2025 => 2025;
#endif
    }
}