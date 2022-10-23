using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public AudioSource am;
    public AudioSource au;
    public AudioSource fire_as;
    public AudioSource au_click;


    
    public void Play()
    {
        Hero.load = false;
        SceneManager.LoadScene("SampleScene");
    }

    public void Load()
    {
        Hero.load = true;
        SceneManager.LoadScene("SampleScene");
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void AudioVolume(float sliderValue)
    {
        am.volume = (float)sliderValue;
        BinaryWriter sw = new BinaryWriter(File.Open("music.dat", FileMode.OpenOrCreate));
        sw.Write((double)sliderValue);
        sw.Close();



    }

    public void EffectsVolume(float sliderValue)
    {
        au.volume = (float)sliderValue;
        fire_as.volume = (float)sliderValue;
        au_click.volume = (float)sliderValue;
        BinaryWriter sw = new BinaryWriter(File.Open("effects.dat", FileMode.OpenOrCreate));
        sw.Write((double)sliderValue); 
        sw.Close();
    }

    public void EffectButtons()
    {
        au.PlayOneShot(au.clip);
    }

    public void ClickButtons()
    {
        au_click.PlayOneShot(au_click.clip);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
}
