using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Data;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649
namespace DefaultNamespace
{
    public class Graph : UnityEngine.MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private GameObject connectionPrefab;
        [SerializeField] private GameObject dividerPrefab;
        [SerializeField] private RectTransform dividerContent;
        [SerializeField] private TMP_Dropdown currencyDropdown;
        [SerializeField] private TMP_Dropdown timeDropdown;


        public void Init()
        {
            gameObject.SetActive(true);
            Show(GetPoints());
        }
        private void Show(List<Vector2>points)
        {
            foreach (Transform tr in content)
                Destroy(tr.gameObject);
            
            foreach (Transform tr in dividerContent)
                Destroy(tr.gameObject);
            
            var graphSize = content.rect.size;
            graphSize = new Vector2(graphSize.x * .9f,graphSize.y * .9f);
           
          
            
            var yMax = points.Max(item => item.y);
            var xMax = points.Max(item => item.x);
            var yMin = points.Min(item => item.y);
            var xMin = points.Min(item => item.x);
            var lastDot = Vector2.zero;
            var currentDot = Vector2.zero;
            foreach (var point in points)
            {
                var gm = Instantiate(dotPrefab, content);
                var cofX = (point.x - (xMin - 1)) / (xMax - (xMin-1));
                
                var pos = new Vector2(cofX  * graphSize.x,point.y/yMax*graphSize.y);

                currentDot = pos;
                SetConnection(lastDot,currentDot);
                lastDot = currentDot;
                (gm.transform as RectTransform).anchoredPosition = pos;
            }
            
            SetDividers(new Vector2(xMax,yMax),new Vector2(xMin,yMin), graphSize );
        }

        private void SetDividers(Vector2 max,Vector2 min,Vector2 graphSize)
        {
            var offset = 100;
            var DividersCountY = (int)(graphSize.y/offset);
            var createdDividersY = new List<int>();
            RectTransform maxSetYDivider = null;
            for (var i = 0; i < DividersCountY; i++)
            {
                var xValue = 0;
                var yValue = (i + 1) * offset;
                var cof = yValue / graphSize.y;
                var count = (int)Math.Round(cof*max.y,MidpointRounding.AwayFromZero);
                if(createdDividersY.Any(item => item==count))
                    continue;
                var gm = Instantiate(dividerPrefab, dividerContent);
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                dividerCount.text = count.ToString(CultureInfo.InvariantCulture); 
                (gm.transform as RectTransform).anchoredPosition = new Vector2(xValue,yValue);
                if (maxSetYDivider == null || yValue > maxSetYDivider.anchoredPosition.y)
                    maxSetYDivider = gm.transform as RectTransform;
                
                createdDividersY.Add(count);
            }

            if (!createdDividersY.Any(item => item == (int) Math.Round(max.y, MidpointRounding.AwayFromZero)))
            {
                var gm = Instantiate(dividerPrefab, dividerContent);
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                dividerCount.text = ((int)Math.Round(max.y, MidpointRounding.AwayFromZero)).ToString();
                (gm.transform as RectTransform).anchoredPosition = new Vector2(0,graphSize.y);
                if (graphSize.y - maxSetYDivider.anchoredPosition.y < 100)
                    Destroy(maxSetYDivider.gameObject);
            }
            
            var createdDividersX = new List<int>();
            var DividersCountX = (int)(graphSize.x/offset);
            RectTransform maxSetXDivider = null;
            for (var i = 0; i < DividersCountX; i++)
            {
                var xValue = (i + 1) * offset;
                var yValue = 0;
                var cof = xValue / graphSize.x;
                var count = (int)Math.Round(min.x -1 +cof*(max.x-min.x+1),MidpointRounding.AwayFromZero);
               
                xValue = (int)((count - (min.x - 1)) / (max.x - (min.x - 1))*graphSize.x);
                if(createdDividersX.Any(item=>item == count))
                    continue;
                var gm = Instantiate(dividerPrefab, dividerContent);
                gm.transform.localEulerAngles = new Vector3(0,0,90);
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                dividerCount.text = count.ToString(CultureInfo.InvariantCulture);
                dividerCount.transform.localEulerAngles = new Vector3(0f,0f,-90f);
                createdDividersX.Add(count);
                (gm.transform as RectTransform).anchoredPosition = new Vector2(xValue,yValue);
                if (maxSetXDivider == null || xValue > maxSetXDivider.anchoredPosition.x)
                    maxSetXDivider = gm.transform as RectTransform;
            }

            if (!createdDividersX.Any(item => item == (int) max.x))
            {
                var gm = Instantiate(dividerPrefab, dividerContent);
                gm.transform.localEulerAngles = new Vector3(0, 0, 90);
                var count = max.x;
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                dividerCount.text = count.ToString(CultureInfo.InvariantCulture);
                dividerCount.transform.localEulerAngles = new Vector3(0f,0f,-90f);
                (gm.transform as RectTransform).anchoredPosition = new Vector2(graphSize.x,0);
                if (graphSize.x - maxSetXDivider.anchoredPosition.x < 65)
                    Destroy(maxSetXDivider.gameObject);
            }

        }
        
        private void SetConnection(Vector2 lastDot, Vector2 currentDot)
        {
            var dir = (currentDot - lastDot);
            var distance = dir.magnitude;
            var angle = Math.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            var dot = Instantiate(connectionPrefab, content).transform as RectTransform;
            dot.anchoredPosition = lastDot;
            dot.localEulerAngles = new Vector3(0f,0f,(float)angle);
            dot.sizeDelta = new Vector2(distance,dot.sizeDelta.y);
        }

        private List<Vector2> GetPoints()
        {
            var points = new List<Vector2>();
            switch (timeDropdown.options[timeDropdown.value].text)
            {
                case "Day":
                {
                    var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;
                    foreach (var dailyTransaction in monthlyTransaction._transactions)
                    {
                        long sum = dailyTransaction._transactions.Sum(transaction => transaction.amount);
                        
                        points.Add(new Vector2(dailyTransaction.day, sum));
                    }

                    break;
                }
                case "Month":
                {
                    var yearlyTransaction = UserDataManager.CurrentYearlyTransactions;
                    foreach (var monthlyTransaction in yearlyTransaction.transactions)
                    {
                        long sum =0;
                        foreach (var dailyTransaction in monthlyTransaction._transactions)
                            sum += dailyTransaction._transactions.Sum(transaction => transaction.amount);
                        
                        points.Add(new Vector2(monthlyTransaction.month, sum));
                    }

                    break;
                }
                case "Year":
                {
                    var yearlyTransactions = UserDataManager.YearlyTransactions;
                    
                    foreach (var yearlyTransaction in yearlyTransactions)
                    { 
                        long sum =0;
                        foreach (var monthlyTransaction in yearlyTransaction.transactions)
                        foreach (var dailyTransaction in monthlyTransaction._transactions)
                            sum+=dailyTransaction._transactions.Sum(transaction => transaction.amount);
                        
                        points.Add(new Vector2(yearlyTransaction.year, sum));
                    }
                    break;
                }
            }
            
            switch (currencyDropdown.options[currencyDropdown.value].text)
            {
                case "USD":
                    for(var i =0;i<points.Count;i++)
                        points[i] = new Vector2(points[i].x,points[i].y/UserDataManager.DollarRate);
                    break;
            }
            return points.OrderBy(item => item.x).ToList();
        }
        
        [Button]
        private void Test()
        {
            var points = new List<Vector2>
            {
                new Vector2(10, 1043),
                new Vector2(11, 2763),
            };
            Show(points);
        }

        [Button()]
        private void Delete()
        {
            foreach (Transform tr in content)
                DestroyImmediate(tr.gameObject);

            foreach (Transform tr in dividerContent)
                DestroyImmediate(tr.gameObject);
        }
    }
}