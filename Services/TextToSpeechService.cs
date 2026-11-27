using System;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;

namespace NotepadApp.Services
{
    /// <summary>
    /// Not içeriklerinin Türkçe ses tonuyla sesli okunmasını yöneten Text-to-Speech (TTS) servisi.
    /// </summary>
    public class TextToSpeechService : IDisposable
    {
        private readonly SpeechSynthesizer? _synthesizer;
        private bool _isSpeaking = false;

        public event Action<bool>? OnSpeakingStateChanged;
        public event Action<string>? OnStatusMessage;

        public bool IsSpeaking => _isSpeaking;

        public TextToSpeechService()
        {
            try
            {
                _synthesizer = new SpeechSynthesizer();
                _synthesizer.SpeakCompleted += (s, e) =>
                {
                    _isSpeaking = false;
                    OnSpeakingStateChanged?.Invoke(false);
                    OnStatusMessage?.Invoke("Sesli okuma tamamlandı.");
                };

                // Türkçe ses ara (Tolga, Yelda vb.)
                var installedVoices = _synthesizer.GetInstalledVoices();
                var turkishVoice = installedVoices.FirstOrDefault(v =>
                    v != null && v.Enabled && v.VoiceInfo != null &&
                    (v.VoiceInfo.Culture.TwoLetterISOLanguageName.Equals("tr", StringComparison.OrdinalIgnoreCase) ||
                     (v.VoiceInfo.Name != null && v.VoiceInfo.Name.Contains("Turkish", StringComparison.OrdinalIgnoreCase)) ||
                     (v.VoiceInfo.Name != null && v.VoiceInfo.Name.Contains("Yelda", StringComparison.OrdinalIgnoreCase)) ||
                     (v.VoiceInfo.Name != null && v.VoiceInfo.Name.Contains("Tolga", StringComparison.OrdinalIgnoreCase))));

                if (turkishVoice != null && !string.IsNullOrEmpty(turkishVoice.VoiceInfo?.Name))
                {
                    _synthesizer.SelectVoice(turkishVoice.VoiceInfo.Name);
                }

                _synthesizer.Rate = 0; // Normal hız
                _synthesizer.Volume = 100;
            }
            catch (Exception ex)
            {
                OnStatusMessage?.Invoke($"TTS başlatılamadı: {ex.Message}");
            }
        }

        /// <summary>
        /// Belirtilen metni asenkron olarak seslendirir.
        /// </summary>
        public void Speak(string text)
        {
            if (_synthesizer == null || string.IsNullOrWhiteSpace(text)) return;

            try
            {
                Stop();
                _isSpeaking = true;
                OnSpeakingStateChanged?.Invoke(true);
                OnStatusMessage?.Invoke("🔊 Not sesli okunuyor...");
                _synthesizer.SpeakAsync(text);
            }
            catch (Exception ex)
            {
                _isSpeaking = false;
                OnSpeakingStateChanged?.Invoke(false);
                OnStatusMessage?.Invoke($"Okuma hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Devam eden sesli okumayı anında durdurur.
        /// </summary>
        public void Stop()
        {
            if (_synthesizer == null) return;

            try
            {
                if (_isSpeaking || _synthesizer.State == SynthesizerState.Speaking)
                {
                    _synthesizer.SpeakAsyncCancelAll();
                }
            }
            catch { }
            finally
            {
                _isSpeaking = false;
                OnSpeakingStateChanged?.Invoke(false);
                OnStatusMessage?.Invoke("Sesli okuma durduruldu.");
            }
        }

        public void Dispose()
        {
            Stop();
            _synthesizer?.Dispose();
        }
    }
}
