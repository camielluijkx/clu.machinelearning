using clu.machinelearning.library;
using static clu.machinelearning.library.ConsoleHelper;

using System.Collections.Generic;

namespace clu.machinelearning.console
{
    class Program
    {
        static void Main(string[] args)
        {
            Initialize();

            ShowMenu(
                new List<MenuItem>
                {
                    new MenuItem(1, "Iris flower classification (dataset)", 
                        IrisFlowerModelRunner.Instance.RunDatasetClassification),
                    new MenuItem(2, "Iris flower classification (individual)", 
                        IrisFlowerModelRunner.Instance.RunIndividualClassification)
                });
        }
    }
}
