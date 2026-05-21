using UnityEditor;
using UnityEngine;

public class AudioImporterPostprocessor : AssetPostprocessor
{
    void OnPreprocessAudio()
    {
        AudioImporter importer = (AudioImporter)assetImporter;

        AudioImporterSampleSettings settings = importer.defaultSampleSettings;

        AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(assetPath);

        // if clip not yet available, just enforce preserve
        settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;

        importer.defaultSampleSettings = settings;
    }

    void OnPostprocessAudio(AudioClip clip)
    {
        if (clip == null)
            return;

        int hz = clip.frequency;

        // only touch clips BELOW 44100
        if (hz < 44100)
        {
            Debug.Log($"{clip.name} was {hz} Hz -> forcing 44100 Hz");

            AudioImporter importer = (AudioImporter)assetImporter;

            AudioImporterSampleSettings settings = importer.defaultSampleSettings;

            settings.sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate;
            settings.sampleRateOverride = 44100;

            importer.defaultSampleSettings = settings;

            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }
}