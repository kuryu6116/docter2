import mysql.connector as mydb
class DatabaseConnect:
    def __init__(self):
        self.conn = mydb.connect(
            host='localhost',
            port='3306',
            user='root',
            password='admin',
            database='ai'
        )

        # Encoding
        #self.conn.set_character_set('utf8')
        self.cur = self.conn.cursor(buffered=True)
        #self.cur.execute('SET NAMES utf8;')
        #self.cur.execute('SET CHARACTER SET utf8;')
        #self.cur.execute('SET character_set_connection=utf8;')  

    def AddDBUser(self,_name,_mail,_pass):

        self.cur.execute("INSERT INTO users (name,point,nega_posi,mail,pass) VALUES (%s, %s,%s, %s,%s)", (_name, 100, 0, _mail,_pass))
        
        #追加したidを文字列として取得
        return str(self.cur.lastrowid)
            
    def AddDBQa(self,_id,_A,_Q):
        self.cur.execute("INSERT INTO qa (answer,question,q_person_id) VALUES (%s, %s, %s)"
                             , (_A, _Q,_id))
        
        
    def ShowDBTables(self):
        #確認用
        self.cur.execute("SELECT * FROM users")
        # 全てのデータを取得
        rows = self.cur.fetchall()
        for row in rows:
            print(type(row))
    
    #コミットするDB終了
    def UpdateDB(self):
        self.conn.commit()
        self.cur.close()
        self.conn.close()
    
    def LoginCheck(self,_name,_pass):
        result_text=""
        self.cur.execute("SELECT * FROM users WHERE name=%s AND pass=%s" , (_name,_pass))
        print(self.cur.rowcount)
        if self.cur.rowcount==1:
            rows = self.cur.fetchall()
            for row in rows:
                result_text=str(row[0]) +","+str(row[2])+","+str(row[3])
        else:
            result_text="error"
        return result_text
    
    #コミットしないDB終了
    def DisconnectDB(self):
        self.cur.close()
        self.conn.close()
    

    def UpdateDBPoint(self,_id,_point,_posi_nega):
        self.cur.execute("UPDATE users SET point=%s nega_posi=%s, where id = %s",(_point,_posi_nega,_id))
    
    def AddEmotion(self,_pict_name,_result_text):
        self.cur.execute("INSERT INTO images(name,emotion) VALUES (%s, %s)"
                             , (_pict_name, _result_text))
        
    

def AddUser(_name,_mail,_pass):
    f=DatabaseConnect()
    return_num=f.AddDBUser(_name,_mail,_pass)
    f.UpdateDB()
    del f
    return return_num

def CheckTable():
    f=DatabaseConnect()
    f.ShowDBTables()
    del f

def Login(_name,_pass):
    f = DatabaseConnect()
    return_text = f.LoginCheck(_name,_pass)
    f.DisconnectDB()
    del f
    return return_text

def UpdatePoint(_id,_point,_posi_nega):
    f = DatabaseConnect()
    f.UpdateDBPoint(_id,_point,_posi_nega)
    f.UpdateDB()
    del f

def AddQa(_id,_A,_Q):
    f = DatabaseConnect()
    f.AddDBQa(_id,_A,_Q)
    f.UpdateDB()
    del f

def AddEmotion(_pict_name,_result_text):
    f = DatabaseConnect()
    f.AddEmotion(_pict_name,_result_text)
    f.UpdateDB()
    del f