using System;

namespace SamplePlugin {
    public partial class RevitCommand {
#if REVIT2016
        public int RevitVersion => 2016;
#elif REVIT2017
        public int RevitVersion => 2017;
#elif REVIT2018
        public int RevitVersion => 2018;
#elif REVIT2019
        public int RevitVersion => 2019;
#elif REVIT2020
        public int RevitVersion => 2020;
#elif REVIT2021
        public int RevitVersion => 2021;
#elif REVIT2022
        public int RevitVersion => 2022;
#elif REVIT2023
        public int RevitVersion => 2023;
#elif REVIT2024
        public int RevitVersion => 2024;
#elif REVIT2025
        public int RevitVersion => 2025;
#else
        public int RevitVersion => throw new NotImplementedException();
#endif
    }
}