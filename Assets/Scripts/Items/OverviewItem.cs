using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class OverviewItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI categoryName;
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI percentCount;
        [SerializeField] private TextMeshProUGUI count;
        [SerializeField] private TextMeshProUGUI comment;
        public void Init(OverviewData data)
        {
            categoryName.text = data.CategoryName;
            slider.value = data.percentageOfAmount;
            percentCount.text = $"<color=red>{data.percentageOfAmount}%</color=red>";
            count.text = data.sum.ToString();
        }
        
        public class OverviewData
        {
            public string IconName;
            public string CategoryName;
            public int sum;
            public int percentageOfAmount;
        }
    }
}