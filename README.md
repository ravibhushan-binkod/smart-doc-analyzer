# 🚀 SmartDocAnalyzer

AI-powered document analysis platform built with **.NET 9** and **Angular 21**, using **Clean Architecture**.  
It processes multiple file formats (PDF, DOCX, TXT, Images via OCR), converts them into text, and enables a **chat-based interface** to interactively analyze, summarize, and extract insights from documents.

---

## ✨ Key Features

- 📄 **Multi-format Support**
  - PDF, DOCX, TXT, Images (OCR)
  
- 🔍 **Text Extraction Pipeline**
  - PDF parsing
  - Word document parsing
  - Image OCR processing
  - Plain text handling

- 🤖 **AI-Powered Chat Interface**
  - Ask questions about uploaded documents
  - Context-aware responses
  - Chat history per document

- 🧠 **Embedding & Semantic Search**
  - Document chunking
  - Vector embeddings for better context retrieval

- ⚡ **Performance & Scalability**
  - In-memory caching / Redis support
  - Retry policies (Polly-ready design)
  - Async processing

- 💾 **Lightweight Storage**
  - LiteDB for document & chat history storage

- 🏗️ **Clean Architecture**
  - Separation of concerns
  - Scalable and testable design

---

## 🧩 Core Components

### 📌 Document Processing
- PDF → `PdfParser`
- DOCX → `WordParser`
- Images → `ImageOcrService`
- TXT → `TextParser`

### 🤖 AI Layer
- `OllamaService` → Local LLM integration
- `EmbeddingService` → Semantic search
- `PromptBuilder` → Context-aware prompts

### 💬 Chat System
- Document-based chat
- Stored in LiteDB
- Supports history retrieval per document

---

## 🔄 Workflow

1. Upload document (PDF / DOCX / TXT / Image)
2. Extract text using parsers / OCR
3. Split into chunks
4. Generate embeddings
5. Store in LiteDB
6. User asks question
7. Relevant chunks fetched
8. AI generates contextual answer
9. Chat history saved

## 🙌 Contribution

Contributions are welcome!
Feel free to fork the repo and submit a pull request.

## 📄 License

This project is licensed under the MIT License.

## 👨‍💻 Author

**Ravi Bhushan**  

## ⭐ Support

If you like this project, give it a ⭐ on GitHub! 
