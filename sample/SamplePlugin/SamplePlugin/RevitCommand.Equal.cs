namespace SamplePlugin {
    public partial class RevitCommand {
#if REVIT_2016
        public int RevitVersion => 2016;
#elif REVIT_2017
        public int RevitVersion => 2017;
#elif REVIT_2018
        public int RevitVersion => 2018;
#elif REVIT_2019
        public int RevitVersion => 2019;
#elif REVIT_2020
        public int RevitVersion => 2020;
#elif REVIT_2021
        public int RevitVersion => 2021;
#elif REVIT_2022
        public int RevitVersion => 2022;
#elif REVIT_2023
        public int RevitVersion => 2023;
#elif REVIT_2024
        public int RevitVersion => 2024;
#else
        public int RevitVersion => throw new NotImplementedException();
#endif
    }
}