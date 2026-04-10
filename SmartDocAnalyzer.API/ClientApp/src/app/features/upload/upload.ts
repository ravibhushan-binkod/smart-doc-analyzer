import { Component, ChangeDetectorRef } from '@angular/core';
import { NgIf } from '@angular/common';
import { DocumentService } from '../../core/services/document.service';
import { DocStateService } from '../../core/services/doc-state.service';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [NgIf],
  templateUrl: './upload.html',
  styleUrls: ['./upload.css']
})
export class UploadComponent {

  file: File | null = null;
  uploading = false;
  progress = 0;
  dragOver = false;

  constructor(private docService: DocumentService,
    private docState: DocStateService, private cdr: ChangeDetectorRef) { }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) this.setFile(file);
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.dragOver = false;

    if (event.dataTransfer?.files.length) {
      this.setFile(event.dataTransfer.files[0]);
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.dragOver = true;
  }

  onDragLeave() {
    this.dragOver = false;
  }

  setFile(file: File) {
    this.file = file;
    this.progress = 0;
  }

  upload() {
    if (!this.file) return;

    this.uploading = true;

    this.docService.upload(this.file).subscribe({
      next: () => {
        this.progress = 100;
        this.uploading = false;
        this.file = null;

        // refresh document list
        this.docState.triggerRefresh();
        this.cdr.detectChanges();
      },
      error: () => {
        this.uploading = false;
      }
    });

    // fake progress (since backend not sending real progress)
    const interval = setInterval(() => {
      if (this.progress < 90) this.progress += 10;
      else clearInterval(interval);
    }, 400);
  }
}
