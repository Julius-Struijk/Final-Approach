using System;
using GXPEngine;
internal class SoundManager
{
    public static SoundManager test = new SoundManager("test.wav", 1 , 1);

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
