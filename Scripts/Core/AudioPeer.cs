using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour {

    AudioSource _audioSource;
    //音階樣本
    public static float[] _samples = new float[512];
    public static float[] _samplesLeft = new float[512]; //左音道
    public static float[] _samplesRight = new float[512];//右音道

    //樣本區段(512音階劃分8段)
    public float[] _freqBand = new float[8];

    //緩衝
    public float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    float[] _freqBandHighest = new float[8];


    /// <summary>取得比例，數值0~1 </summary>
    public static float[] _audioBand = new float[8];
    /// <summary>取得比例，數值0~1 </summary>
    public static float[] _audioBandBuffer = new float[8];

    ///<summary>振幅(比例 0~1)</summary>
    public static float _Amplitude, _AmplitudeBuffer;
    float _AmplitudeHighest;

    /// <summary>幫助微調_freqBandHighest，避免音調太低，視覺效果不好</summary>
    public float _audioProfile;

    //在editor選單(drop down menu)上可供選擇聲道
    public enum _channel{Stero,Left,Right};
    public _channel channel = new _channel();

    void Start() {
        _audioSource = GetComponent<AudioSource>();

        AudioProfile(_audioProfile);
    }

    void Update() {
        MakeFrequencyBands();
        GetSpctrumAudioSource();
        BandBuffer();
        CreatAudioBands();
        GetAmplitude();
    }

    void AudioProfile(float audioProfile) {
        for (int i=0;i<8;i++) {
            _freqBandHighest[i] = audioProfile;
        }
    }

    //取得振幅 (比例)
    void GetAmplitude() {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer=0;

        for (int i=0;i<8;i++) {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }

        //取最大
        if (_CurrentAmplitude>_AmplitudeHighest) {
            _AmplitudeHighest = _CurrentAmplitude;
        }
        _Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        _AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }

    //取得數值比例
    void CreatAudioBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                //得最大值
                _freqBandHighest[i] = _freqBand[i];
            }


            //取得比例數值(0~1)
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);

        }

    }

    void GetSpctrumAudioSource() {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);

        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman); //0和1代表左右聲道
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);
    }

    //緩衝
    void BandBuffer() {
        for (int g=0;g<8;g++) {

            if (_freqBand[g]>_bandBuffer[g]) {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }

            if (_freqBand[g]<_bandBuffer[g]) {

                _bandBuffer[g] -= _bufferDecrease[g];

                //越掉越快
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands() {
        //22050/512=43hz per sample 以下公式

        int count = 0;

        for (int i=0;i<8;i++) {
            float average = 0;

            int sampleCount = (int)Mathf.Pow(2, i)*2;

            if (i==7) {
                sampleCount += 2;
            }

            for (int j=0;j<sampleCount;j++) {

                //3種聲道處理
                if (channel == _channel.Stero)
                {
                    average += (_samplesLeft[count] + _samplesRight[count] )* (count + 1);
                }
                if (channel==_channel.Left) {
                    average += _samplesLeft[count]* (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);
                }


                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }
    }
}
