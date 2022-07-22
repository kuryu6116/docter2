import pycurl
import ast
import io
import os
from PIL import Image
from array import array

from flask import Flask,jsonify
from flask import request

import ChatBot
import GuessText
import EmotionAnalysis
import FaceApi
import DBConnect
import ErrorText
import subprocess

app = Flask(__name__)
app.config["JSON_AS_ASCII"] = False

result_text=""
result=""

#def StartServer():
  #cmd = 'ngrok http -host-header="0.0.0.0:5000" -subdomain=aitalker -auth="uryu:kohei" --region=jp 5000'
  #returncode = subprocess.Popen(cmd)

def PostDataToDict(_text):
  return_dic = ast.literal_eval(_text)
  return return_dic

@app.route("/face", methods=["POST"])
def EmotionFace():
  try:
    #userid=request.files['userid']
    file = request.files['image']
    # Read the image via file.stream
    img = Image.open(file.stream)
    rotate_image = img.rotate(270)
    DIR='./PNG/temp'
    file_int=len([name for name in os.listdir(DIR) if os.path.isfile(os.path.join(DIR, name))])
    file_pass='./PNG/temp/tmp'+str(file_int+1)+'.png'
    rotate_image.save(file_pass)
    result = FaceApi.FaceCheck(file_pass)
    DBConnect.AddEmotion(str(file_int+1),result)
    print(result)

  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/AI",methods=["POST"])
def AIResult():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    result=GuessText.bert(dic['talk'])
    DBConnect.AddQa(int(dic['userid']),result,dic['talk'])
  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/chat",methods=["POST"])
def ChatResult():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    print(dic)
    result = ChatBot.talk_api(dic['talk'])
    DBConnect.AddQa(int(dic['userid']),result,dic['talk'])
  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/emotion",methods=["POST"])
def EmotionResult():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    result = EmotionAnalysis.Emotion(dic['talk'])
    DBConnect.AddQa(int(dic['userid']),result,dic['talk'])
  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/login",methods=["POST"])
def Login():
  try:
    #print(request.get_data(as_text=True))
    dic=PostDataToDict(request.get_data(as_text=True))
    print(dic)
    result = DBConnect.Login(dic['username'],dic['password'])
    #result_text=OpenTextToString(request.get_data(as_text=True))
  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/entry",methods=["POST"])
def Entry():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    print(dic)
    result = DBConnect.AddUser(dic['username'],dic['mail'],dic['password'])
  except Exception as e:
    result = "error"
    ErrorText.AddText(str(e))
  finally:
    return result

@app.route("/error",methods=["POST"])
def Error():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    ErrorText.AddText(dic['error'])
  except Exception as e:
    Error.AddText(str(e))
  finally:
    return "ok"
  

@app.route("/point",methods=["POST"])
def Point():
  try:
    dic=PostDataToDict(request.get_data(as_text=True))
    DBConnect.UpdatePoint(int(dic['userid']),int(dic['point']),float(dic['nega_posi']))
  except Exception as e:
    ErrorText.AddText(str(e))
  finally:
    return "ok"

@app.route("/check",methods=["POST"])
def CheckStart():
  return "ok"
  


if __name__ == '__main__':
  #StartServer()
  app.run(host='localhost', port=5000, threaded=True)