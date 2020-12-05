// author:KIPKIPS
// time:2020.12.05 18:31:52
// describe:BasePanel 字典扩展方法类
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtension {
    //获取字典中key对应的value值,得到返回key 否则null
    public static Tvalue TryGet<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key) {
        Tvalue value;
        dict.TryGetValue(key, out value);
        return value;
    }
}
