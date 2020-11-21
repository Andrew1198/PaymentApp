using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Managers;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
namespace DefaultNamespace
{
    public class Graph : UnityEngine.MonoBehaviour
    {
        [SerializeField] private RectTransform content;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private GameObject connectionPrefab;
        [SerializeField] private GameObject dividerPrefab;
        [SerializeField] private RectTransform dividerContent;


        public void Init()
        {
            gameObject.SetActive(true);
            List<Vector2> points = new List<Vector2>();
            var monthlyTransaction = UserDataManager.CurrentMonthlyTransaction;
            foreach (var dailyTransaction in monthlyTransaction._transactions)
            {
                long sum=0;
                foreach (var transaction in dailyTransaction._transactions)
                    sum += transaction._count;
                foreach (var transaction in dailyTransaction.bankTransactions)
                    sum += transaction.amount;
                
                points.Add(new Vector2(dailyTransaction.day,sum));
            }
            
            points = points.OrderBy(item => item.x).ToList();
            Show(points);
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
                var cofX = (point.x - (xMin-1)) / (xMax - (xMin-1));
                var pos = new Vector2(cofX  * graphSize.x,point.y/yMax  * graphSize.y);

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
            for (var i = 0; i < DividersCountY; i++)
            {
                var gm = Instantiate(dividerPrefab, dividerContent);
                var xValue = 0;
                var yValue = (i + 1) * offset;
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                var count = Math.Round(max.y * (yValue / graphSize.y),1,MidpointRounding.AwayFromZero);
                dividerCount.text = count.ToString(CultureInfo.InvariantCulture); 
                (gm.transform as RectTransform).anchoredPosition = new Vector2(xValue,yValue);
            }
            
            var DividersCountX = (int)(graphSize.x/offset);
            for (var i = 0; i < DividersCountX; i++)
            {
                var gm = Instantiate(dividerPrefab, dividerContent);
                gm.transform.localEulerAngles = new Vector3(0,0,90);
                var xValue = (i + 1) * offset;
                var yValue = 0;
                var dividerCount = gm.transform.Find("Count").GetComponent<TextMeshProUGUI>();
                dividerCount.transform.localEulerAngles = new Vector3(0f,0f,-90f);
                var cof = xValue / graphSize.x;
                var count = Math.Round(min.x+(max.x-min.x)*cof,1,MidpointRounding.AwayFromZero);
                dividerCount.text = count.ToString(CultureInfo.InvariantCulture); 
                (gm.transform as RectTransform).anchoredPosition = new Vector2(xValue,yValue);
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
        

        [Button]
        private void Test()
        {
            var points = new List<Vector2>
            {
                new Vector2(2017, 2),
                new Vector2(2018, 5),
                new Vector2(2019, 7),
                new Vector2(2020, 9),
                new Vector2(2021, 10)
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