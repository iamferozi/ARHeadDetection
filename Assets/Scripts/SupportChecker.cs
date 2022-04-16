using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This example shows how to check for AR support before the ARSession is enabled.
    /// For ARCore in particular, it is possible for a device to support ARCore but not
    /// have it installed. This example will detect this case and prompt the user to install ARCore.
    /// To test this feature yourself, use a supported device and uninstall ARCore.
    /// (Settings > Search for "ARCore" and uninstall or disable it.)
    /// </summary>
    public class SupportChecker : MonoBehaviour
    {
        [SerializeField]
        ARSession m_Session;


        public ARSession session
        {
            get { return m_Session; }
            set { m_Session = value; }
        }

        [SerializeField]
        Text m_LogText;

        public Text logText
        {
            get { return m_LogText; }
            set { m_LogText = value; }
        }

        [SerializeField]
        Button m_InstallButton;
        [SerializeField]
        Button m_CloseButton;
        [SerializeField]
        GameObject m_SupportPanel;

        public Button installButton
        {
            get { return m_InstallButton; }
            set { m_InstallButton = value; }
        }
        public Button closeButton
        {
            get { return m_CloseButton; }
            set { m_CloseButton = value; }
        }
        public GameObject supportPanel
        {
            get { return m_SupportPanel; }
            set { m_SupportPanel = value; }
        }

        void Log(string message)
        {
            m_LogText.text = $"{message}\n";
        }

        IEnumerator CheckSupport()
        {
            SetInstallButtonActive(false);

            Log("Checking for AR support...");

            yield return ARSession.CheckAvailability();

            yield return new WaitForSeconds(2f);

            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                Log("Your device supports AR, but requires a software update.");
                yield return new WaitForSeconds(2f);
                Log("Attempting install...");
                yield return ARSession.Install();
            }

            if (ARSession.state == ARSessionState.Ready)
            {
                Log("Your device supports AR!");
                yield return new WaitForSeconds(2f);
                Log("Starting AR session...");
                yield return new WaitForSeconds(2f);
                Log("Press Close to continue...");
                m_CloseButton.interactable = true;
                // To start the ARSession, we just need to enable it.
                m_Session.enabled = true;
                PlayerPrefs.SetInt("SupportAvaliable", 1);

            }
            else
            {
                switch (ARSession.state)
                {
                    case ARSessionState.Unsupported:
                        Log("Your device does not support AR. Try another device.");
                        m_CloseButton.interactable = true;
                        m_CloseButton.onClick.AddListener(closeApplication);
                        PlayerPrefs.SetInt("SupportAvaliable", 0);
                        break;
                    case ARSessionState.NeedsInstall:
                        Log("The software update failed, or you declined the update.");

                        // In this case, we enable a button which allows the user
                        // to try again in the event they decline the update the first time.
                        SetInstallButtonActive(true);
                        break;
                }

                //Log("\n[Start non-AR experience instead]");

                //
                // Start a non-AR fallback experience here...
                //
            }
        }

        void SetInstallButtonActive(bool active)
        {
            if (m_InstallButton != null)
                m_InstallButton.gameObject.SetActive(active);
        }

        IEnumerator Install()
        {
            SetInstallButtonActive(false);

            if (ARSession.state == ARSessionState.NeedsInstall)
            {
                yield return new WaitForSeconds(1.5f);

                Log("Attempting install...");
                yield return ARSession.Install();

                if (ARSession.state == ARSessionState.NeedsInstall)
                {
                    yield return new WaitForSeconds(1.5f);
                    Log("The software update failed, or you declined the update.");
                    SetInstallButtonActive(true);
                }
                else if (ARSession.state == ARSessionState.Ready)
                {
                    yield return new WaitForSeconds(1.5f);
                    Log("Success! Starting AR session...");
                    yield return new WaitForSeconds(2f);
                    Log("Press Close to continue...");
                    m_Session.enabled = true;
                    m_CloseButton.interactable = true;
                    PlayerPrefs.SetInt("SupportAvaliable", 1);
                }
            }
            else
            {
                Log("Error: ARSession does not require install.");
            }
        }

        public void OnInstallButtonPressed()
        {
            StartCoroutine(Install());
        }

        void OnEnable()
        {
            if (PlayerPrefs.GetInt("SupportAvaliable") == 0)
            {
                StartCoroutine(CheckSupport());
                m_SupportPanel.SetActive(true);
            }
            else
            {
                m_SupportPanel.SetActive(false);
                m_Session.enabled = true;
            }
        }

        void closeApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
#if !UNITY_EDITOR
            Application.Quit();
#endif
        }
    }
}