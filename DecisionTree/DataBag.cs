using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class DataBag
    {
        public DataBag(List<DataInstance> firstList)
        {
            dataList = firstList;
            AttributesLeft = DataSet.Attributes.Count - 1;
        }
        public DataBag(DataBag oldBag, int attribute, int value)
        {
            dataList = oldBag.dataList.Where(instance => instance.Data[attribute] == value).ToList();
            AttributesLeft = oldBag.AttributesLeft - 1;
        }

        public List<DataInstance> dataList;
        public int AttributesLeft;
        public bool IsAtomic => AttributesLeft == 0 || dataList.All(instance => instance.Qualifier == dataList.First().Qualifier);
    }
}
