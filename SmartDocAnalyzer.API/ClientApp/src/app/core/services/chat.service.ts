import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class ChatService {
  constructor(private api: ApiService) { }

  ask(data: any) {
    return this.api.post<any>('chat/ask-ollama', data);
  }

  getHistory(docId: string) {
    return this.api.get<any[]>(`chat/history/${docId}`);
  }

  clearHistory(docId: string) {
    return this.api.delete(`chat/history/${docId}`);
  }
}
