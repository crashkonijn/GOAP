using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrashKonijn.Agent.Core
{
    public interface IMonoBehaviour
    {
        bool Equals(object other);
        int GetHashCode();
        string ToString();
        int GetInstanceID();
        string name { get; set; }
        HideFlags hideFlags { get; set; }
        Transform transform { get; }
        GameObject gameObject { get; }
        string tag { get; set; }
        bool enabled { get; set; }
        bool isActiveAndEnabled { get; }
        bool useGUILayout { get; set; }
        Component GetComponent(Type type);
        T GetComponent<T>();
        Component GetComponent(string type);
        bool TryGetComponent(Type type, out Component component);
        bool TryGetComponent<T>(out T component);
        Component GetComponentInChildren(Type t, bool includeInactive);
        Component GetComponentInChildren(Type t);
        T GetComponentInChildren<T>(bool includeInactive);
        T GetComponentInChildren<T>();
        Component[] GetComponentsInChildren(Type t, bool includeInactive);
        Component[] GetComponentsInChildren(Type t);
        T[] GetComponentsInChildren<T>(bool includeInactive);
        void GetComponentsInChildren<T>(bool includeInactive, List<T> result);
        T[] GetComponentsInChildren<T>();
        void GetComponentsInChildren<T>(List<T> results);
        Component GetComponentInParent(Type t, bool includeInactive);
        Component GetComponentInParent(Type t);
        T GetComponentInParent<T>(bool includeInactive);
        T GetComponentInParent<T>();
        Component[] GetComponentsInParent(Type t, bool includeInactive);
        Component[] GetComponentsInParent(Type t);
        T[] GetComponentsInParent<T>(bool includeInactive);
        void GetComponentsInParent<T>(bool includeInactive, List<T> results);
        T[] GetComponentsInParent<T>();
        Component[] GetComponents(Type type);
        void GetComponents(Type type, List<Component> results);
        void GetComponents<T>(List<T> results);
        T[] GetComponents<T>();
        bool CompareTag(string tag);
        void SendMessageUpwards(string methodName, object value, SendMessageOptions options);
        void SendMessageUpwards(string methodName, object value);
        void SendMessageUpwards(string methodName);
        void SendMessageUpwards(string methodName, SendMessageOptions options);
        void SendMessage(string methodName, object value);
        void SendMessage(string methodName);
        void SendMessage(string methodName, object value, SendMessageOptions options);
        void SendMessage(string methodName, SendMessageOptions options);
        void BroadcastMessage(string methodName, object parameter, SendMessageOptions options);
        void BroadcastMessage(string methodName, object parameter);
        void BroadcastMessage(string methodName);
        void BroadcastMessage(string methodName, SendMessageOptions options);
        bool IsInvoking();
        bool IsInvoking(string methodName);
        void CancelInvoke();
        void CancelInvoke(string methodName);
        void Invoke(string methodName, float time);
        void InvokeRepeating(string methodName, float time, float repeatRate);
        Coroutine StartCoroutine(string methodName);
        Coroutine StartCoroutine(string methodName, object value);
        Coroutine StartCoroutine(IEnumerator routine);
        Coroutine StartCoroutine_Auto(IEnumerator routine);
        void StopCoroutine(IEnumerator routine);
        void StopCoroutine(Coroutine routine);
        void StopCoroutine(string methodName);
        void StopAllCoroutines();
    }
}