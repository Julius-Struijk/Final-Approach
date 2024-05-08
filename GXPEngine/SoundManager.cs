using System;
using GXPEngine;
internal class SoundManager
{
    public static SoundManager test = new SoundManager("Sounds/test.mp3", 0 , 0);

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
}
