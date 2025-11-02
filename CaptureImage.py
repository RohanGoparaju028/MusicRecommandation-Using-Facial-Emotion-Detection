import cv2 
import sys
def captureImage():
    face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')
    cap = cv2.VideoCapture(0)
    window = 'Look at the camera'
    face_img = None
    while True:
        has_frames,frames = cap.read()
        if not has_frames:
            sys.exit("Failed to Read the image frame by frame")
        cv2.imshow(window,frames)
        gray = cv2.cvtColor(frames,cv2.COLOR_BGR2GRAY)
        faces = face_cascade.detectMultiScale(gray,scaleFactor=1.05,minNeighbors=3,minSize=(30,30))
        for (x,y,w,h) in faces:
            cv2.rectangle(frames,(x,y),(x+w,y+h),(0,255,0),2)
        key = cv2.waitKey(1)
        if key == ord('q') or key == ord('Q'):
            if len(faces)>0:
                (x,y,w,h) = faces[0]
                face_img = frames[y:y+h,x:x+w]
                cv2.imwrite('inputImage.jpg',face_img)
            else:
                print("No image exist")
            break
    cap.release()
    cv2.destroyWindow(window)
    return face_img