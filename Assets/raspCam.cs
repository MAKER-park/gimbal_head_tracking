using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class raspCam : MonoBehaviour
{

    public RawImage rawImage;
    private UnityWebRequest request;

    public float GetRate = 0.15f;
    private float nextGet = 0;

    private void Start()
    {
        StartCoroutine(GetStream());
    }

    private IEnumerator GetStream()
    {
        while (true)
        {
            try{
                request = UnityWebRequestTexture.GetTexture("http://cloud.park-cloud.co19.kr:30/?action=snapshot");
                Debug.Log("Getting stream...");
                
            }
            catch{
                Debug.Log("Cannot getting stream");
            }
            Debug.Log("Waiting.. 1");
                
            yield return request.SendWebRequest();
            Debug.Log("Waiting.. 2");
            while( !request.isDone) {
                yield return null;
            }

            if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
            }
            else
            {
                
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                rawImage.texture = texture;
                Debug.Log("Successfully Getting stream...");
            }
        }
    }
    void Update(){
        if(Time.time > nextGet){
            StartCoroutine(GetStream());
            nextGet = Time.time + GetRate;
        }
    }
}



// using System.IO;
// using System.Threading;
// using System.Collections;
// using System.Collections.Generic;
// using System.Net;
// using UnityEngine;

// public class raspCam : MonoBehaviour
// {
//     [SerializeField] bool tryOnStart = false;
//     [SerializeField] string defaultStreamURL = "http://cloud.park-cloud.co19.kr:30/?action=stream";
    
//     [SerializeField] RenderTexture renderTexture;

//     float RETRY_DELAY = 5f;
//     int MAX_RETRIES = 3;
//     int retryCount = 0;


//     byte[] nextFrame = null;

//     Thread worker;
//     int threadID = 0;

//     static System.Random randu; // I use my own System.Random instead of the shared UnityEngine.Random to avoid collisions
//     List<BufferedStream> trackedBuffers = new List<BufferedStream>();

//     // Start is called before the first frame update
//     void Start()
//     {
//         randu = new System.Random(Random.Range(0, 65536));
//         if (true)
//             StartStream(defaultStreamURL);
//     }

//     private void Update()
//     {
//         if (nextFrame != null)
//         {
//             SendFrame(nextFrame);
//             nextFrame = null;
//         }
//     }

//     private void OnDestroy()
//     {
//         foreach (var b in trackedBuffers)
//         {
//             if (b != null)
//                 b.Close();
//         }
//     }

//     public void StartStream(string url)
//     {
//         retryCount = 0;
//         StopAllCoroutines();
//         foreach (var b in trackedBuffers)
//             b.Close();

//         worker = new Thread(() => ReadMJPEGStreamWorker(threadID = randu.Next(65536), url));
//         worker.Start();
//     }

//     void ReadMJPEGStreamWorker(int id, string url)
//     {
//         var webRequest = WebRequest.Create(url);
//         webRequest.Method = "GET";
//         List<byte> frameBuffer = new List<byte>();

//         int lastByte = 0x00;
//         bool addToBuffer = false;

//         BufferedStream buffer = null;
//         try
//         {
//             Stream stream = webRequest.GetResponse().GetResponseStream();
//             buffer = new BufferedStream(stream);
//             trackedBuffers.Add(buffer);
//         }
//         catch (System.Exception ex)
//         {
//             Debug.LogError(ex);
//         }
//         int newByte;
//         while (buffer != null)
//         {
//             if (threadID != id) return; // We are no longer the active thread! stop doing things damnit!
//             if (!buffer.CanRead)
//             {
//                 Debug.LogError("Can't read buffer!");
//                 break;
//             }

//             newByte = -1;

//             try
//             {
//                 newByte = buffer.ReadByte();
//             }
//             catch
//             {
//                 break; // Something happened to the stream, start a new one
//             }

//             if (newByte < 0) // end of stream or failure
//             {
//                 continue; // End of data
//             }

//             if (addToBuffer)
//                 frameBuffer.Add((byte)newByte);

//             if (lastByte == 0xFF) // It's a command!
//             {
//                 if (!addToBuffer) // We're not reading a frame, should we be?
//                 {
//                     if (IsStartOfImage(newByte))
//                     {
//                         addToBuffer = true;
//                         frameBuffer.Add((byte)lastByte);
//                         frameBuffer.Add((byte)newByte);
//                     }
//                 }
//                 else // We're reading a frame, should we stop?
//                 {
//                     if (newByte == 0xD9)
//                     {
//                         frameBuffer.Add((byte)newByte);
//                         addToBuffer = false;
//                         nextFrame = frameBuffer.ToArray();
//                         frameBuffer.Clear();
//                     }
//                 }
//             }

//             lastByte = newByte;
//         }

//         if (retryCount < MAX_RETRIES)
//         {
//             retryCount++;
//             Debug.LogFormat("[{0}] Retrying Connection {1}...", id, retryCount);
//             foreach (var b in trackedBuffers)
//                 b.Dispose();
//             trackedBuffers.Clear();
//             worker = new Thread(() => ReadMJPEGStreamWorker(threadID = randu.Next(65536), url));
//             worker.Start();
//         }
//     }

//     bool IsStartOfImage(int command)
//     {
//         switch (command)
//         {
//             case 0x8D:
//                 Debug.Log("Command SOI");
//                 return true;
//             case 0xC0:
//                 Debug.Log("Command SOF0");
//                 return true;
//             case 0xC2:
//                 Debug.Log("Command SOF2");
//                 return true;
//             case 0xC4:
//                 Debug.Log("Command DHT");
//                 break;
//             case 0xD8:
//                 //Debug.Log("Command DQT");
//                 return true;
//             case 0xDD:
//                 Debug.Log("Command DRI");
//                 break;
//             case 0xDA:
//                 Debug.Log("Command SOS");
//                 break;
//             case 0xFE:
//                 Debug.Log("Command COM");
//                 break;
//             case 0xD9:
//                 Debug.Log("Command EOI");
//                 break;
//         }
//         return false;
//     }

//     void SendFrame(byte[] bytes)
//     {
//         Texture2D texture2D = new Texture2D(2, 2);
//         texture2D.LoadImage(bytes);
//         //Debug.LogFormat("Loaded {0}b image [{1},{2}]", bytes.Length, texture2D.width, texture2D.height);

//         if (texture2D.width == 2)
//             return; // Failure!

//         Graphics.Blit(texture2D, renderTexture);
//         Destroy(texture2D); // LoadImage discards the previous buffer, so there's no point in trying to reuse it
//     }
// }