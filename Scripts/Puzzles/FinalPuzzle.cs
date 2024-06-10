using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalPuzzle : MonoBehaviour
{
    public int[] password = { 0, 6, 1, 0 };

    private FinalPuzzlePasswordUI ui;

    private Coroutine noiseCamCoroutine;
    private AudioSource[] audioSource;
    [SerializeField] private float fearAmount = 30f;
    [SerializeField] private float clearDuration = 8f;

    public void Awake()
    {
        ui = GameObject.Find("HUD").GetComponentInChildren<FinalPuzzlePasswordUI>();
        audioSource = GetComponents<AudioSource>();
    }

    public void FindPassword(int puzzleNum)
    {
        ui.UpdatePassword(puzzleNum, password[puzzleNum]);
    }

    public void ClearPuzzle()
    {
        // ���� ����� ����
        AudioManager.Instance.PlayBgm(false);
        // ���� Ŭ���� ����
        audioSource[1].volume = 0.7f;
        audioSource[1].Play();
        // �Ͼ� ���̵� �ƿ�
        StartCoroutine(ShowClearFadeOut());
        // ���� Ŭ���� ��
        Invoke("ClearScene", clearDuration);
        // Ŀ�� ����� ����
        GameManager.Instance.PlayerController.ControlLocked();
    }

    public void FailPuzzle()
    {
        Invoke("InvokeFailPuzzle", 1f);
    }

    public void InvokeFailPuzzle()
    {
        // ī�޶� ��鸲
        noiseCamCoroutine = StartCoroutine(noiseCamera());
        // ���� �Ҹ�
        audioSource[0].volume = 0.8f;
        audioSource[0].PlayOneShot(audioSource[0].clip);
        // ���� ������ ���
        GameManager.Instance.PlayerController.gameObject.GetComponent<FearEventHandler>().AddFear(fearAmount);
    }

    private IEnumerator noiseCamera()
    {
        PuzzleManager.Instance.noiseCam.Priority = 20;

        yield return new WaitForSeconds(3f);

        PuzzleManager.Instance.noiseCam.Priority = 0;

        StopCoroutine(noiseCamCoroutine);
    }

    private void ClearScene()
    {

        SceneManager.LoadScene("ClearScene");
    }

    IEnumerator ShowClearFadeOut()
    {

        float elapsed = 0f; // ��� �ð�
        float startAlpha = 0f; // ���� ���İ�
        float targetAlpha = 1f; // ��ǥ ���İ� (1f�� 255f�� ǥ���� ���İ�)

        while (elapsed < clearDuration)
        {
            elapsed += Time.deltaTime; // ��� �ð��� ����
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / clearDuration); // ���� ���� ���� ����
            PuzzleManager.Instance.clearFadeOut.color = new Color(1f, 1f, 1f, alpha); // ���� ���� (1f�� 255f�� ǥ���� ���� ��)
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ���� ���� ��ǥ ���� ������ ����
        PuzzleManager.Instance.clearFadeOut.color = new Color(1f, 1f, 1f, targetAlpha);
        PuzzleManager.Instance.clearFadeOut = null;
    }


}