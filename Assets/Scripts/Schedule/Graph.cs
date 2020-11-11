using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        
[Button()]
        private void Test()
        {
            var graphSize = content.rect.size;
            graphSize = new Vector2(graphSize.x * .9f,graphSize.y * .9f);
            var Points = new List<Vector2>{
                new Vector2(1,5),
                new Vector2(2,15),
                new Vector2(3,20),
                new Vector2(4,30),
                new Vector2(5,50),
                new Vector2(6,75),
                new Vector2(7,100)
            };
            /*var Points = new List<Vector2>();
            for (var i = 1; i < 25; i++)
            {
                Points.Add(new Vector2
                {
                    x = i,
                    y = Random.Range(0,100)
                });
            }*/
            
            var yMax = Points.Max(item => item.y);
            var xMax = Points.Max(item => item.x);
            var lastDot = Vector2.zero;
            var currentDot = Vector2.zero;
            foreach (var point in Points)
            {
                var gm = Instantiate(dotPrefab, content);
                var pos = new Vector2(point.x/xMax  * graphSize.x,point.y/yMax  * graphSize.y);

                currentDot = pos;
                SetConnection(lastDot,currentDot);
                lastDot = currentDot;
                (gm.transform as RectTransform).anchoredPosition = pos;
            }
            
            SetDividers(new Vector2(xMax,yMax),graphSize );
        }

        private void SetDividers(Vector2 max,Vector2 graphSize)
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
                var count = Math.Round(max.x * (xValue / graphSize.x),1,MidpointRounding.AwayFromZero);
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
        

        [Button()]
        private void Delete()
        {
            foreach (Transform tr in content)
            {
                DestroyImmediate(tr.gameObject);
            }
            foreach (Transform tr in dividerContent)
            {
                DestroyImmediate(tr.gameObject);
            }
        }
    }
}