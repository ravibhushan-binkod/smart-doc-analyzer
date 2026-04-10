import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({ providedIn: 'root' })
export class DocumentService {
  constructor(private api: ApiService) { }

  getAll() {
    return this.api.get<any[]>('document/list');
  }

  upload(file: File) {
    const formData = new FormData();
    formData.append('file', file);
    return this.api.post<any>('document/upload', formData);
  }

  delete(documentId: string) {
    return this.api.delete<any>(`document/${documentId}`);
  }
}
