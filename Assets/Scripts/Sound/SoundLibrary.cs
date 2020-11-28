// author:KIPKIPS
// time:2020.11.28 22:55:05
// describe:音频资源库
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {
    public SoundGroup[] soundGroups;
    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();//资源字典
    public AudioClip GetClipFromName(string name) {
        if (groupDictionary.ContainsKey(name)) {
            AudioClip[] sounds = groupDictionary[name];
            return sounds[Random.Range(0, sounds.Length)];
        }
        return null;
    }
    private void Awake() {
        foreach (SoundGroup group in soundGroups) {
            groupDictionary.Add(group.groupID, group.audioClipGroup);
        }
    }

    //音频资源组
    [System.Serializable]
    public class SoundGroup {
        public string groupID;
        public AudioClip[] audioClipGroup;
    }
}
