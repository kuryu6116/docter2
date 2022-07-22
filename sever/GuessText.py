import MeCab
import subprocess
import re
import torch
from transformers import BertTokenizer, BertForMaskedLM, BertConfig
import numpy as np

import MecabAnalysis

def bert(_text):
  tokenized_list=[]
  tokenized_text=[]
  return_text=""
  config = BertConfig.from_json_file('./BERT/config.json')
  model = BertForMaskedLM.from_pretrained('./BERT/pytorch_model.bin', config=config)
  bert_tokenizer = BertTokenizer('./BERT/vocab.txt',
  do_lower_case=False, do_basic_tokenize=False)
  
  tokenized_text = MecabAnalysis.MecabToList(_text)
  
  
  #形態素解析した結果を表示
  tokenized_text.insert(0, '[CLS]')
  tokenized_list=[]
  count=0
  masked_index = 0
  for text in tokenized_text:
      count+=1
      if text=='何':
          tokenized_list.append('[MASK]')
          masked_index=count-1
      elif '。' in text:
          tokenized_list.append(text)
          tokenized_list.append('[SEP]')
          count+=2
      else:
          tokenized_list.append(text)

  tokens = bert_tokenizer.convert_tokens_to_ids(tokenized_list)
  tokens_tensor = torch.tensor([tokens])
  print(masked_index)

  model.eval()
  
  tokens_tensor = tokens_tensor.to('cuda')
  model.to('cuda')
  
  with torch.no_grad():
    outputs = model(tokens_tensor)
    predictions = outputs[0]
  
    _, predicted_indexes = torch.topk(predictions[0, masked_index], k=3)
    predicted_tokens = bert_tokenizer.convert_ids_to_tokens(predicted_indexes.tolist())
    
    for list_text in predicted_tokens:
      return_text+=list_text+','
    print(return_text)
    return return_text