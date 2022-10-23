using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class StartSettings : MonoBehaviour
{

    public AudioSource music;
    public AudioSource fire;
    public AudioSource menu_buttons_effects;
    public AudioSource menu_buttons_click;
    public Slider slider_music;
    public Slider slider_effects;
    void Start()
    {
        BinaryReader sr = new BinaryReader(File.Open("music.dat", FileMode.Open));
        BinaryReader sr_ = new BinaryReader(File.Open("effects.dat", FileMode.Open));
        if(sr_ != null && sr !=null)
        {
            music.volume = (float)sr.ReadDouble();
            sr.Close();
            slider_music.value = music.volume;
            fire.volume = (float)sr_.ReadDouble();
            sr_.Close();
            menu_buttons_effects.volume = fire.volume;
            menu_buttons_click.volume = fire.volume;
            slider_effects.value = fire.volume;
        }
        sr.Close();
        sr_.Close();
    }
    void Update()
    {
        
    }
}
