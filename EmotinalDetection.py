from fer import FER
import cv2
def detectEmotion(frame):
    detector = FER(mtcnn=True)
    rgb = cv2.cvtColor(frame,cv2.COLOR_BGR2RGB)
    result = detector.top_emotion(rgb)
    if result:
        dominant_emotion,_ = result
        return dominant_emotion
    else:
        return None