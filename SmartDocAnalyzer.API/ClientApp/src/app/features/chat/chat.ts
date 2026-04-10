import { Component, ChangeDetectorRef, ViewChild, ElementRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, NgClass, DatePipe } from '@angular/common';
import { ChatService } from '../../core/services/chat.service';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [FormsModule, NgFor, NgIf, NgClass, DatePipe],
  templateUrl: './chat.html',
  styleUrls: ['./chat.css']
})
export class ChatComponent {

  selectedDocId = localStorage.getItem('docId') || '';
  selectedDocName = localStorage.getItem('docName') || '';
  messages: any[] = [];
  question = '';
  loading = false;

  constructor(private chatService: ChatService, private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    window.addEventListener('docChanged', () => {
      this.selectedDocId = localStorage.getItem('docId') || '';
      this.selectedDocName = localStorage.getItem('docName') || '';
      this.messages = [];
      if (this.selectedDocId) setTimeout(() => this.loadHistory(this.selectedDocId));
      //this.cdr.detectChanges();
    });
  }

  clearSelection() {
    this.selectedDocId = '';
    this.selectedDocName = '';
    this.messages = [];
    localStorage.removeItem('docId');
    localStorage.removeItem('docName');
    window.dispatchEvent(new Event('docChanged'));
  }

  send() {
    if (!this.question || !this.selectedDocId) return;

    this.messages.push({ documentId: this.selectedDocId, role: 'user', message: this.question });
    setTimeout(() => this.scrollToBottom(), 50);
    this.loading = true;

    this.chatService.ask({
      documentId: this.selectedDocId,
      question: this.question,
      provider: 'ollama'
    }).subscribe({
      next: (res) => {
        console.log('API Response:', res);
        this.question = '';
        // RELOAD FROM LiteDB 
        this.loadHistory(this.selectedDocId);
      },
      error: (err) => {
        this.loading = false;
        console.error('API Error:', err);
      },
      complete: () => {
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.question = '';
  }

  loadHistory(docId: string) {
    this.chatService.getHistory(docId).subscribe(res => {
      this.messages = res || [];

      setTimeout(() => this.scrollToBottom(), 50);
      this.cdr.detectChanges();
    });
  }

  @ViewChild('chatContainer') chatContainer!: ElementRef;

  scrollToBottom() {
    try {
      this.chatContainer.nativeElement.scrollTop =
        this.chatContainer.nativeElement.scrollHeight;
    } catch { }
  }
}
