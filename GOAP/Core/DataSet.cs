/**
 * Copyright 2017 Sword GC
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * Author: Peter Klooster | CrashKonijn
 * Project: https://github.com/crashkonijn/GOAP
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SwordGC.AI.Goap
{
    /// <summary>
    /// Object that holds all the information that the GOAP might need
    /// </summary>
    public class DataSet
    {
        /// <summary>
        /// Holds al the data
        /// </summary>
        public  Dictionary<string, bool> data { get; private set; }

        /// <summary>
        /// Creates a new default dataset
        /// </summary>
        public DataSet ()
        {
            data = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Creates a new copy of a dataset
        /// </summary>
        /// <param name="copy"></param>
        public DataSet (DataSet copy)
        {
            data = copy.data;
        }

        /// <summary>
        /// Sets the data of the input key
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The (new) value</param>
        public void SetData (string key, bool value)
        {
            if (data.ContainsKey(key))
            {
                data[key] = value;
            }
            else
            {
                data.Add(key, value);
            }
        }

        /// <summary>
        /// Checks the value of a key with the input value
        /// </summary>
        /// <param name="key">The key of the data</param>
        /// <param name="value">The comparison value</param>
        /// <returns>Are the input and data value the same</returns>
        public bool Equals (string key, bool value)
        {
            if (data.ContainsKey(key))
            {
                return data[key] == value;
            }
            else
            {
                return false;
            }
        }
    }
}