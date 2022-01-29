using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public static class ListExtensions {
    
    public static void Shuffle<T> (this List<T> list) {
        int n = list.Count;
        for (int i = 0; i < n; i++) {
            int r = i + Random.Range(0, (n - i));
            T t = list[r];
            list[r] = list[i];
            list[i] = t;
        }
    }

    public static T RandomElement<T> (this List<T> list) {
        return list[Random.Range(0, list.Count)];
    }   
    
     /// <summary>
    /// Adds an element to a list ONLY if the list does not already contain that element.
    /// </summary>
    /// <param name="list">The list being extended.</param>
    /// <param name="addedElement">The element being added to the list.</param>
    /// <typeparam name="T"></typeparam>
    public static bool AddIfUnique<T>(this List<T> list, T addedElement) {
         if (!list.Contains(addedElement)) {
             list.Add(addedElement);
             return true;
         }
         return false;
     }
    
    /// <summary>
    /// Removes all entries in a list that meet specific criteria.
    /// </summary>
    /// <param name="list">The list being pruned.</param>
    /// <param name="where">The criteria being met.</param>
    /// <typeparam name="T"></typeparam>
    public static void StrikeWhere<T>(this List<T> list, Func<T, bool> where) {
        for (int i = list.Count - 1; i >= 0; i--) {
            if (where(list[i])) {
                list.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Attempts to introduce an item to a list by replacing the null element nearest to the target position. If
    /// the list contains no null elements, the item is not introduced.
    /// </summary>
    /// <param name="list">The list being filled.</param>
    /// <param name="targetPosition">The item's preferred position.</param>
    /// <param name="item">The item being introduced.</param>
    /// <param name="forwardFirst">If true, the search prefers positions AFTER the target position. Otherwise, it
    /// prefers positions BEFORE the target position. In either case, nearness is always prioritized over direction
    /// </param>
    /// <typeparam name="T"></typeparam>
    public static void AddToNearestNullPosition<T>(this List<T> list, int targetPosition, T item, bool forwardFirst = true) {
        for (int i = 0; i < list.Count; i++) {
            if (forwardFirst && targetPosition + i < list.Count) {
                if (list[targetPosition + i] == null) {
                    list[targetPosition + i] = item;
                    return;
                }
            }
            if (targetPosition - i >= 0) {
                if (list[targetPosition - i] == null) {
                    list[targetPosition - i] = item;
                    return;
                }
            }
            if (!forwardFirst && targetPosition + i < list.Count) {
                if (list[targetPosition + i] == null) {
                    list[targetPosition + i] = item;
                    return;
                }
            }
        }
    }
        
}
