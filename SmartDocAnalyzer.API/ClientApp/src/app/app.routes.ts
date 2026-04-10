import { Routes } from '@angular/router';
import { ChatComponent } from './features/chat/chat';
import { DocumentListComponent } from './features/documents/document-list';

export const routes: Routes = [
  { path: '', component: ChatComponent },
  { path: 'documents', component: DocumentListComponent }
];
