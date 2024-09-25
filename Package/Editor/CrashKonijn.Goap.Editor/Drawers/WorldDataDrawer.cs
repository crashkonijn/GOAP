using System;
using System.Collections.Generic;
using System.Linq;
using CrashKonijn.Agent.Core;
using CrashKonijn.Agent.Runtime;
using CrashKonijn.Goap.Core;
using UnityEngine;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor
{
    public class WorldDataDrawer : VisualElement
    {
        public WorldDataDrawer(ILocalWorldData worldData)
        {
            this.name = "world-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("World Data"));
                
                var root = new VisualElement();
                
                var scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
                scrollView.Add(root);
                
                card.schedule.Execute(() =>
                {
                    root.Clear();
                    
                    root.Add(this.CreateTable(new string[] { "Key", "Value", "Age" }, this.GetValues(worldData)));
                }).Every(500);
                
                card.Add(scrollView);
            });
            
            this.Add(card);
        }

        private string[][] GetValues(ILocalWorldData worldData)
        {
            return this.GetWorldValues(worldData).Concat(this.GetTargetValues(worldData)).ToArray();
        }

        private string[][] GetWorldValues(ILocalWorldData worldData)
        {
            return worldData.States.Select(pair => this.GetValues(pair.Key, pair.Value)).ToArray();
        }

        private string[][] GetTargetValues(ILocalWorldData worldData)
        {
            return worldData.Targets.Select(pair => this.GetValues(pair.Key, pair.Value)).ToArray();
        }

        private string[] GetValues<T>(Type key, IWorldDataState<T> dataState)
        {
            var local = dataState.IsLocal ? "Local" : "Global";
            
            return new string[]
            {
                $"{key.GetGenericTypeName()} ({local})",
                this.GetValueText(dataState),
                this.GetText(dataState.Timer)
            };
        }

        private string GetText(ITimer timer)
        {
            var secondsAgo = timer.GetElapsed();
            
            return $"{secondsAgo:0.00}s";
        }

        private string GetValueText<T>(IWorldDataState<T> dataState)
        {
            if (dataState.Value is int intValue)
            {
                return $"{intValue}";
            }
            
            if (dataState.Value is ITarget target)
            {
                return $"{target.Position}";
            }

            return "";
        }
        
        private VisualElement CreateTable(string[] headers, string[][] rows)
        {
            var tableContainer = new VisualElement();
            tableContainer.style.flexDirection = FlexDirection.Column;
            tableContainer.style.flexGrow = 1;

            // Create header row
            var headerRow = new VisualElement();
            headerRow.style.flexDirection = FlexDirection.Row;
            headerRow.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1); // Dark background for headers
            headerRow.style.height = 25;
            headerRow.style.paddingLeft = 5;
            headerRow.style.paddingRight = 5;

            // Set column widths
            float[] columnWidths = { 200, 150, 50 }; // Fixed width for each column

            // Create header columns
            for (var i = 0; i < headers.Length; i++)
            {
                var headerLabel = new Label(headers[i]);
                headerLabel.style.width = columnWidths[i];  // Set fixed width for each column
                headerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                headerLabel.style.color = Color.white;
                headerLabel.style.paddingLeft = 10;
                headerLabel.style.paddingRight = 10;
                // headerLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
                // headerLabel.style.flexWrap = Wrap.Wrap;
                headerLabel.style.flexGrow = 1;  // Allow columns to expand equally
                headerRow.Add(headerLabel);
            }

            tableContainer.Add(headerRow);

            // Create data rows
            foreach (var row in rows)
            {
                var dataRow = new VisualElement();
                dataRow.style.flexDirection = FlexDirection.Row;
                dataRow.style.paddingLeft = 10;
                dataRow.style.paddingRight = 10;
                dataRow.style.height = 20;
                dataRow.style.borderBottomColor = new Color(0.8f, 0.8f, 0.8f, 1);
                dataRow.style.borderBottomWidth = 1;
                dataRow.style.flexGrow = 1;

                for (var i = 0; i < row.Length; i++)
                {
                    var dataLabel = new Label(row[i]);
                    dataLabel.style.width = columnWidths[i];  // Set fixed width for each column
                    dataLabel.style.paddingLeft = 10;
                    dataLabel.style.paddingRight = 10;
                    dataLabel.style.flexWrap = Wrap.Wrap;
                    // dataLabel.style.unityTextAlign = TextAnchor.MiddleCenter;  // Center align the text
                    dataLabel.style.flexGrow = 1;
                    dataRow.Add(dataLabel);
                }

                tableContainer.Add(dataRow);
            }

            return tableContainer;
        }
    }
}