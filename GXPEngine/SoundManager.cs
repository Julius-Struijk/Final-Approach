using System;
using GXPEngine;
public class SoundManager
{
    public static SoundManager Death_sound = new SoundManager("Sounds/Death_sound.wav", 0, 0); //Implemented
    public static SoundManager Hitting_surface_at_high_speed_sound = new SoundManager("Sounds/Hitting_surface_at_high_speed_sound.wav", 0, 0); //Implemented
    public static SoundManager player_taking_damage = new SoundManager("Sounds/player_taking_damage.wav", 0, 0); //Implemented
    public static SoundManager rotate_left = new SoundManager("Sounds/rotate_left.wav", 0, 0); //Implemented
    public static SoundManager rotate_right = new SoundManager("Sounds/rotate_right.wav", 0, 0); //Implemented
    public static SoundManager spring_sound = new SoundManager("Sounds/spring_sound.wav", 0, 0); //Implemented

    private Sound storedSound;
    private float defaultVolume;
    private uint defaultChannel;

    public SoundManager(String fileName, float defaultVolume, float defaultChannel)
    {
        storedSound = new Sound(fileName);
        this.defaultVolume = defaultVolume;
        this.defaultChannel = this.defaultChannel;
    }

    public void play(float volume, uint chanel)
    {
        storedSound.Play(false, chanel, volume, 0);
    }

    //SoundManager.test.play(0.5f, 0); to play the sounds
}
