def AddText(_text):
    with open("./error.txt", mode='a',encoding= 'utf-8') as f:
        f.write(_text+'\n')
        f.close