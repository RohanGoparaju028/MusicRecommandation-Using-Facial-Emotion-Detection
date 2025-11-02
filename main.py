import CaptureImage as ci
import EmotinalDetection as ed
import sys
if __name__=='__main__':
    frame = ci.captureImage()
    if frame is None:
        sys.exit("No image is captured")
    emotion = ed.detectEmotion(frame)
    if emotion is not None:
        print(emotion)
    else:
        print("No emotion")