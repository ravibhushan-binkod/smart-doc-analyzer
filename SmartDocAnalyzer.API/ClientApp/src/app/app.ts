import { Component, signal } from '@angular/core';
import { ChatComponent } from './features/chat/chat';
import { DocumentListComponent } from './features/documents/document-list';
import { UploadComponent } from './features/upload/upload';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [ChatComponent, DocumentListComponent, UploadComponent],
  templateUrl: './app.html'
})
export class AppComponent {
  protected readonly title = signal('ClientApp');
}
