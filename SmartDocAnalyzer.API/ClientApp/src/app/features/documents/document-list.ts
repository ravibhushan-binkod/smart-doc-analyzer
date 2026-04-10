import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { DocumentService } from '../../core/services/document.service';
import { DocStateService } from '../../core/services/doc-state.service';

@Component({
  selector: 'app-document-list',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './document-list.html',
  styleUrls: ['./document-list.css']
})
export class DocumentListComponent implements OnInit {

  loading = true;
  documents: any[] = [];
  selectedId = localStorage.getItem('docId') || '';

  constructor(private docService: DocumentService,
    private docState: DocStateService, private cdr: ChangeDetectorRef) { }

  ngOnInit() {
    setTimeout(() => this.load());
    this.docState.refresh$.subscribe(() => {
      setTimeout(() => this.load());
    });
  }

  load() {
    this.loading = true;

    this.docService.getAll().subscribe({
      next: (res) => {
        console.log('API Response:', res); 
        this.documents = res;
        this.loading = false;

        // notify chat
        window.dispatchEvent(new Event('docChanged'));
        this.cdr.detectChanges(); 
      },
      error: (err) => {
        console.error('API Error:', err);
        this.loading = false;
      }
    });
  }

  select(doc: any) {
    this.selectedId = doc.documentId;
    localStorage.setItem('docId', doc.documentId);
    localStorage.setItem('docName', doc.fileName);

    // notify chat
    window.dispatchEvent(new Event('docChanged'));
  }

  deleteDoc(doc: any, event: Event) {
    event.stopPropagation();

    if (!confirm('Are you sure you want to delete this document?')) return;

    this.docService.delete(doc.documentId).subscribe(res => {

      this.documents = this.documents.filter(d => d.documentId !== doc.documentId);

      if (this.selectedId === doc.documentId) {
        this.selectedId = '';
        localStorage.removeItem('docId');
        localStorage.removeItem('docName');
        this.docState.triggerRefresh();
      }

      this.cdr.detectChanges();
    });
  }
}
