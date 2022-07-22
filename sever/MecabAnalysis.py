import MeCab
import subprocess
import re

"""
形態素解析を行いたい文字列を、形態素解析後のリストで返す処理
"""
def MecabToList(_text):
    m = MeCab.Tagger ("-Ochasen")
    mecab=m.parse(_text)
    lines = mecab.split('\n')
    items = (re.split('[\t]',line) for line in lines)
    mecab_list=[]
    tokenized_text=[]
    [tokenized_text.append(item[0]) for item in items if(item[0] !='' and item[0] !='EOS')]
    return tokenized_text