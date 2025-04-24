/**
 * ZurafWorks projelerindeki temel global fonksiyonları içerir.
 */
const zuraf = {

    //üzerine işlem yapılacak gridin Idsi
    operationGridId: "",


    //CRUD İşlemleri

    // GET BY ID
    /**
     * Id'ye göre öğe getirme işlemi yapar. İsteğe bağlı olarak modal açılabilir.
     * 
     * @param {string} controllerUrl - İstek yapılacak controller'ın URL'i.
     * @param {string} id - İşlem yapılacak öğenin Id'si.
     * @param {string} targetGrid - Hedef gridin ID veya selector bilgisi.
     * @param {string} modal - Dönüş tipi ne? Varsayılan: modal. "modal", "data", "offcanvas"
     * @returns {string|null} - Eğer modal açılacaksa null döner, aksi takdirde içerik döner.
     */
    getById: function (controllerUrl, id, targetGrid, type = "modal") {
        if (window.event) {
            window.event.preventDefault();
        }

        debugger;
        zuraf.operationGridId = targetGrid;

        //var data = {
        //    Id: id ?? 0
        //};

        // AJAX işlemi
        $.ajax({
            url: controllerUrl.replace("{id}", id),  // Dinamik olarak id'yi URL'ye yerleştir
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(id),
            success: function (result) {
                // Eğer modal açılacaksa
                if (type !== "data") {
                    $('#partialEditContent').html(result);  // İçeriği ekle
                    if (type === "modal") {
                        $('#editModal').modal({  // Modalı aç
                            backdrop: 'static',
                            keyboard: false
                        }).modal('show');
                    }
                    if (type === "offcanvas") {
                        var offcanvas = new bootstrap.Offcanvas(document.getElementById('editModal'));
                        offcanvas.show();
                    }
                } else {
                    // Modal açılmayacaksa, içerik döndürülür
                    return result;  // İçeriği döndür
                }
            },
            error: function (xhr) {
                toastr.error(xhr.responseJSON?.message || 'Beklenmeyen bir hata oluştu.', 'Hata', { timeOut: 5000 });
            }
        });
    },

    //SUBMIT FORM
    /**
     * Belirtilen formu sunucuya gönderir ve sonucu toastr ile kullanıcıya bildirir.
     * @param {string} formId - Gönderilecek formun ID'si.
     */
    submitForm: function (formId, specialfields) {

        $('body').append(`
        <div id="loading-overlay" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5); z-index: 9998;">
               <div style="position: fixed;top: 50%;left: 50%;" class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <style>

</style>
        </div>
    `);

        // Form verilerini bir JSON nesnesi olarak topluyoruz
        var formDataArray = $('#' + formId).serializeArray();
        var formDataObject = {};
        $.each(formDataArray, function (i, field) {
            formDataObject[field.name] = field.value;
        });
        debugger;
        // specialfields içerisindeki her bir öğe için işleme yapıyoruz
        $.each(specialfields, function (key, value) {
            var name = value.name; // name özelliğini alıyoruz

            if (value.type === "checkbox") {
                // Checkbox için değeri alıyoruz (checked durumu)
                formDataObject[name] = $('#' + key).prop('checked') ? true : false;
            } else if (value.type === "textEditor") {
                // Text editor için değeri alıyoruz (editor içeriği)
                var editorContent = $('#' + key).find('.ql-editor').html(); // .ql-editor öğesini buluyoruz
                formDataObject[name] = editorContent; // HTML içeriğini string olarak ekliyoruz
            } else if (value.type === "fileupload") {
                // File upload için değeri alıyoruz (seçilen dosya)
                var fileInput = $('#' + key)[0];
                if (fileInput.files.length > 0) {
                    formDataObject[name] = fileInput.files[0]; // İlk dosyayı alıyoruz
                }
            }
        });

        // FormData nesnesi oluşturuyoruz
        var formData = new FormData();
        $.each(formDataObject, function (key, value) {
            formData.append(key, value);
        });
        $('#loading').css('display', 'block');  // Yükleme animasyonunu göster


        // AJAX isteğini başlatıyoruz
        $.ajax({
            url: $('#' + formId).attr('action'),
            type: 'POST',
            data: formData, // FormData nesnesi gönderiliyor
            processData: false, // jQuery'nin veriyi işlememesi için
            contentType: false, // İçerik türünü ayarlamıyoruz
            success: function (result) {
                $('#loading-overlay').remove();
                zuraf.closeModal('#editModal');
                toastr.success("İşlem Başarılı", 'Başarılı', { timeOut: 5000 });
                zuraf.reloadDataGrid(zuraf.operationGridId);
            },
            //error: function (result) {
            //    toastr.error(result.message, 'Hata', { timeOut: 5000 });
            //}
            error: function (xhr) {
                $('#loading-overlay').remove();
                if (xhr.status === 400) {
                    let response = xhr.responseJSON;

                    if (response) {
                        $(".is-invalid").removeClass("is-invalid");
                        $(".invalid-feedback").html("");

                        $.each(response, function (key, errorMessages) {
                            let input = $("[name='" + key + "']");
                            input.addClass("is-invalid");

                            let errorDiv = input.siblings(".invalid-feedback");
                            if (errorDiv.length) {
                                errorDiv.html(Array.isArray(errorMessages) ? errorMessages.join("<br>") : errorMessages);
                            }

                            if (key == "ErrorDetail") {
                                toastr.error((Array.isArray(errorMessages) ? errorMessages.join("<br>") : errorMessages), "Hata", { timeOut: 5000 })
                            }
                        });

                    } else {
                        console.error("Beklenmeyen hata formatı:", xhr.responseText);
                        toastr.error(xhr.responseText, "Hata", { timeOut: 5000 })
                    }
                }
            }
        });
    },

    //SUBMIT BODY
    /**
     * Belirtilen data sunucuya gönderir ve sonucu toastr ile kullanıcıya bildirir.
     * @param {string} url - Gönderilecek url.
     * @param {string} data - Gönderilecek data.
     * @param {function} callback - Başarılı olduğunda çalıştırılacak fonksiyon.
     */
    submitBody: function (url, data, callback) {
        $('body').append(`
        <div id="loading-overlay" style="position: fixed; top: 0; left: 0; width: 100%; height: 100%; background-color: rgba(0, 0, 0, 0.5); z-index: 9998;">
               <div style="position: fixed;top: 50%;left: 50%;" class="spinner-border text-primary" role="status">
          <span class="visually-hidden">Loading...</span>
        </div>
        <style>

</style>
        </div>
    `);
        // AJAX isteğini başlatıyoruz
        $.ajax({
            url: url,
            type: 'POST',
            data: JSON.stringify(data), // JSON formatına çeviriyoruz
            contentType: 'application/json; charset=utf-8', // İçerik türü JSON olarak ayarlandı
            success: function (result) {
                $('#loading-overlay').remove();
                toastr.success(result.message, 'Başarılı', { timeOut: 5000 });
                if (callback) callback(); // Callback fonksiyonu çağrılıyor
            },
            error: function (result) {
                $('#loading-overlay').remove();
                toastr.error(result.message, 'Hata', { timeOut: 5000 });
            }
        });
    },

    // DELETE BY ID
    /**
     * Id'ye göre öğeyi silme işlemi yapar. Silme işlemi onaylanarak yapılır.
     * 
     * @param {string} controllerUrl - İstek yapılacak controller'ın URL'i.
     * @param {string} id - Silinecek öğenin Id'si.
     * @param {string} targetGrid - Hedef gridin ID veya selector bilgisi.
     * @param {boolean} confirmDelete - Silme onayı gerekip gerekmediğini belirler. Varsayılan: false.
     * @returns {void}
     */
    deleteById: function (controllerUrl, id, targetGrid, confirmDelete = false) {
        // Silme onayı isteği
        if (confirmDelete) {
            // Toastr ile onay kutusu göster
            toastr.options = {
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "preventDuplicates": true,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000"

            };

            // Toastr mesajını göster ve butonları ekle
            var toastrMessage = toastr.error(
                '<strong style="font-size:large">Silmek istediğinizden emin misiniz?</strong><br>' +
                '<button class="btn btn-secondary me-1 mb-1 reject" type="button" style="float: right;">Vazgeç</button>' +
                '<button class="btn btn-danger me-1 mb-1 accept" type="button" style="float: right;">Sil</button>',
                {
                    "positionClass": "toast-top-full-width",
                    "timeOut": 0,  // No auto-dismiss
                    "extendedTimeOut": 0,  // No extension of time on hover
                }
            );


            var $toastrElement = $('.toast');

            // Butonlara tıklama olayları ekle
            $toastrElement.on('click', '.accept', function () {
                executeDelete();  // Silme işlemi kabul edildi
                toastr.remove();  // Toastr mesajını kaldır
                toastr.clear();
            });

            $toastrElement.on('click', '.reject', function () {
                toastr.remove();  // Silme işlemi reddedildi, toastr mesajını kaldır
                toastr.clear();
            });

            return; // to prevent the rest of the function from executing until user confirms
        }

        // Silme işlemi onaylandıysa, devam et
        function executeDelete() {

            debugger;
            // AJAX işlemi
            $.ajax({
                url: controllerUrl,  // Dinamik olarak URL
                type: 'POST',  // Silme işlemi için POST isteği
                contentType: 'application/json',
                data: JSON.stringify(id),  // Silinecek öğenin ID'sini JSON formatında gönder
                success: function (result) {
                    // Silme işlemi başarılı ise, grid'i güncelle
                    zuraf.reloadDataGrid(targetGrid)
                    // Sunucudan gelen mesajı toastr ile göster
                    toastr.success(result.message || 'Öğe başarıyla silindi.');
                },
                error: function (xhr) {
                    // Sunucudan gelen hata mesajını toastr ile göster
                    toastr.error(xhr.responseJSON?.message || 'Beklenmeyen bir hata oluştu.');
                }
            });
        }
    },


    /**
   * Bu fonksiyon, belirtilen formu içeren modal'ı kapatır.
   * Ayrıca, form sıfırlama işlemi isteğe bağlı olarak yapılabilir.
   * 
   * @param {string} formId - Modal içinde bulunan formun seçici (ID).
   * @param {boolean} [reset=false] - Form sıfırlanacak mı? Varsayılan değer false'dur.
   */
    closeModal: function (formId, reset = false) {
        // Modal'ı kapat
        var popup = $(formId); // 'modal' fonksiyonu, bootstrap ile modal'ı gizler.
        //popup.css("display", "none").attr("aria-hidden", "true");
        popup.modal('hide');


        // Eğer reset parametresi true ise, formu sıfırla
        if (reset) {
            //$(formId)[0].reset(); // Form elemanları sıfırlanır (input, select vb.).
        }
    },

    /**
  * reloadDataGrid fonksiyonu, belirli bir DataGrid bileşenini yeniler.
  * 
  * @param {string} gridId - Yenilenecek DataGrid'in ID'si (örn. "#myGrid").
  */
    reloadDataGrid: function (gridId) {
        // Parametre olarak alınan gridId ile DataGrid örneğini alıyoruz
        const grid = $(gridId).dxDataGrid("instance"); // dxDataGrid instance'ını al

        // Grid'i yeniden yükleyerek görünümü tazeliyoruz
        grid.refresh(); // Grid'i yenile
    },


    //CONFIGURATION FUNCTIONS(Ön Ayar Fonksiyonları

    //LOAD LIBRARIES
    /**
     * Belirtilen kütüphaneleri koşullu olarak sayfaya yükler.
     * 
      * Desteklenen kütüphaneler ve işlevleri:
         * - quill: Rich Text Editor oluşturmak için kullanılır.
         *     - CSS: `quill.snow.css`, `zurafworks/text-editor.css`
         *     - JS: `quill.core.js`, `quill.js`
         *
         * - quillI18n: Quill zengin metin düzenleyici için uluslararasılaştırma (i18n) desteği ekler.
         *     - JS: `quill-i18n.min.js`, `langs/tr.js`
         * 
         * - highlight: Metin düzenleyici içinde kod parçacıklarını vurgulamak için kullanılır
         *     - CSS: `atom-one-dark.min.css`
         *     - JS: `highlight.min.js`
         * 
         * - katex: Matematiksel ifadeleri LaTeX formatında işlemek için kullanılır.
         *     - CSS: `katex.min.css`
         *     - JS: `katex.min.js`
         
     * @param {Array<string>} libraries - Yüklenecek kütüphanelerin isimleri.
         */
    loadLibraries: function (libraries) {
        // Kütüphane ayarları
        const libraryConfig = {
            quill: {
                css: [
                    "/texteditor/quill/dist/quill.snow.css",
                    "/css/zurafworks/text-editor.css"
                ],
                js: [
                    "/texteditor/quill/dist/quill.core.js",
                    "/texteditor/quill/dist/quill.js"
                ]
            },
            highlight: {
                css: [
                    "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/styles/atom-one-dark.min.css"
                ],
                js: [
                    "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.9.0/highlight.min.js"
                ]
            },
            katex: {
                css: [
                    "https://cdn.jsdelivr.net/npm/katex@0.16.9/dist/katex.min.css"
                ],
                js: [
                    "https://cdn.jsdelivr.net/npm/katex@0.16.9/dist/katex.min.js"
                ]
            },
            quillI18n: {
                js: [
                    "https://cdn.jsdelivr.net/npm/quill-i18n/dist/quill-i18n.min.js",
                    "https://cdn.jsdelivr.net/npm/quill-i18n/dist/langs/tr.js"
                ]
            },
            cropper: {
                js: [
                    "/cropper/cropper.min.js"
                ]
            }
        };

        // Kütüphane yükleyici fonksiyon
        function loadResource(type, url) {
            if (type === 'css') {
                const link = document.createElement('link');
                link.rel = 'stylesheet';
                link.href = url;
                document.head.appendChild(link);
            } else if (type === 'js') {
                const script = document.createElement('script');
                script.src = url;
                document.body.appendChild(script);
            }
        }

        // Seçili kütüphaneleri yükle
        libraries.forEach(lib => {
            const config = libraryConfig[lib];
            if (config) {
                config.css && config.css.forEach(url => loadResource('css', url));
                config.js && config.js.forEach(url => loadResource('js', url));
            }
        });
    },

    // FUNCTIONS FOR SPECIAL ELEMENTS (Özel Elementler için Fonksiyonlar)

    //GET TEXT EDITOR
    /**
    * Text Editor'ü (Quill) başlatır.
    * 
     * option seçenekleri:
         * - education: normal text editorun yanında matematiksel ifadeler de içerir.
        *
         * - common: sadece görsel düzenlemeler barındıran temel ayarlar.
    * 
    * @param {string} editorId - Editörünün HTML id'si.
    * @param {string} option - Ayar Türü.
    */
    getTextEditor: function (editorId, option) {
        // Global toolbar options
        let toolbarOptions = [];
        let syntaxStatus = false;
        let placeholderText = "Metin içeriği...";

        // options'a göre toolbar'ı ayarlıyoruz
        switch (option) {
            case 'education':
                toolbarOptions = [
                    [{ 'header': '1' }, { 'header': '2' }, { 'font': [] }],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    [{ 'align': [] }],
                    ['bold', 'italic', 'underline'],
                    ['link'],
                    [{ 'indent': '-1' }, { 'indent': '+1' }],
                    [{ 'color': [] }, { 'background': [] }],
                    ['blockquote', 'code-block'],
                    ['image', 'video']
                ];

                syntaxStatus = true;

                placeholderText = "Soru İçeriği";
                break;
            case 'common':
                toolbarOptions = [
                    [{ 'header': '1' }, { 'header': '2' }, { 'font': [] }],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    ['bold', 'italic', 'underline'],
                    [{ 'align': [] }],
                    ['link'],
                    [{ 'indent': '-1' }, { 'indent': '+1' }],
                    [{ 'color': [] }, { 'background': [] }],
                    ['blockquote', 'code-block']
                ];
                break;
            default:
                toolbarOptions = [
                    [{ 'header': '1' }, { 'header': '2' }, { 'font': [] }],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    ['bold', 'italic', 'underline'],
                    ['link']
                ];
                break;
        }

        // Quill editor başlatma
        const quill = new Quill(`#${editorId}`, {
            modules: {
                syntax: syntaxStatus, // Varsayılan olarak syntax false olacak
                toolbar: toolbarOptions
            },
            placeholder: placeholderText,
            theme: 'snow',
        });

        return quill;
    },

    //HANDLE IMAGE UPLOAD
    /**
     * Yüklenen görseli işleyip uygun boyut ve formatta düzenler.
     * 
     * @param {string} fileInputId - Dosya input'unun id'si (input[type="file"]).
     * @param {string} imgElementId - Görselin gösterileceği img elementinin id'si.
     * @param {string} hiddenInputId - Görselin base64 formatındaki verisinin tutulacağı hidden input'un id'si.
     * @param {string} type - Görselin türü (örn. 'profile', 'background', 'other').
     * @returns {void} - Görselin işlenip base64 formatında hidden input'a ve img elementine aktarılması.
     */
    uploadImage: function (fileInputId, imgElementId, hiddenInputId, type) {
        const sizeConfig = {
            'profile': { minWidth: 100, minHeight: 100, maxWidth: 500, maxHeight: 500, format: 'jpeg' },
            'pack': { minWidth: 1400, minHeight: 1120, maxWidth: 1400, maxHeight: 1120, format: 'jpeg' },
            'background': { minWidth: 150, minHeight: 150, maxWidth: 800, maxHeight: 800, format: 'png' },
            'other': { minWidth: 200, minHeight: 200, maxWidth: 600, maxHeight: 600, format: 'png' },
        };

        const { minWidth, minHeight, maxWidth, maxHeight, format } = sizeConfig[type] || { minWidth: 192, minHeight: 192, maxWidth: 500, maxHeight: 500, format: 'jpeg' };

        const inputFile = document.getElementById(fileInputId);
        if (inputFile.files.length === 0) {
            console.log("No file selected.");
            return;
        }

        const reader = new FileReader();
        reader.onload = function (e) {
            const tempImg = new Image();
            tempImg.src = e.target.result;

            tempImg.onload = function () {
                let width = tempImg.width, height = tempImg.height;
                const aspectRatio = width / height;

                // Görsel boyutlarını kontrol ediyoruz.
                if (width > maxWidth) { width = maxWidth; height = maxWidth / aspectRatio; }
                if (height > maxHeight) { height = maxHeight; width = maxHeight * aspectRatio; }
                if (width < minWidth) { width = minWidth; height = minWidth / aspectRatio; }
                if (height < minHeight) { height = minHeight; width = minHeight * aspectRatio; }

                const canvas = document.createElement('canvas');
                const ctx = canvas.getContext('2d');

                // Görseli en boy oranı koruyarak kırpma işlemi
                const cropWidth = width > maxWidth ? maxWidth : width;
                const cropHeight = height > maxHeight ? maxHeight : height;

                // Canvas'ı belirlenen hedef boyutta kırpmak için ayar yapıyoruz
                canvas.width = cropWidth;
                canvas.height = cropHeight;

                // Kırpma işlemi için canvas'a çiziyoruz
                const offsetX = (tempImg.width - cropWidth) / 2;
                const offsetY = (tempImg.height - cropHeight) / 2;
                ctx.drawImage(tempImg, offsetX, offsetY, cropWidth, cropHeight, 0, 0, cropWidth, cropHeight);

                // Kırpılmış görseli Base64 formatında alıp img tag'ine yolluyoruz
                document.getElementById(imgElementId).src = canvas.toDataURL(`image/${format}`);

                // Görseli blob olarak alıp yeni bir File nesnesi oluşturuyoruz.
                canvas.toBlob(blob => {
                    const file = new File([blob], "resized.jpg", { type: `image/${format}` });
                    inputFile.files = new DataTransfer().items.add(file).files;  // Dosya yüklemeyi simüle ediyoruz.
                }, `image/${format}`);
            };
        };
        reader.readAsDataURL(inputFile.files[0]);
    }
    ,

    //COLOR PICKER LIMITER
    /**
     * Belirtilen renk türüne göre en yakın rengi bulur ve seçilen rengi,
     * belirtilen input elemanının değerine atar.
     * 
     * @param {string} selectedColor - Seçilen renk (hex kodu olarak).
     * @param {string} colorType - Renk türü ('pastel' ya da 'vibrant').
     * @param {string} elementId - Değeri değiştirilmesi gereken input elemanının id'si.
     */
    limitColor: function (selectedColor, colorType, elementId) {
        // Renk türüne göre kullanılacak renk listesi belirleniyor
        let colorList;
        switch (colorType) {
            case 'pastel':
                // Pastel renk listesi
                colorList = [
                    "#FF6F61", "#FF7F50", "#FF7F32", "#FF9A8B", "#FFFF66", "#FFDF00", "#FFF44F",
                    "#8AFF80", "#A4FF60", "#A4E1B1", "#80C7FF", "#87CEFA", "#A2D8FF", "#7FFFD4",
                    "#B39DDB", "#E5A3D5", "#D8A7D3", "#FF77A9", "#FFB6C1", "#FF69B4", "#5FFFBF",
                    "#7DFFB5", "#B4C400", "#6699FF", "#C2A2D1", "#F4A300", "#D3D3D3", "#C72C48",
                    "#6699FF", "#F8C8DC", "#F1C6E4", "#E7A1C2", "#F7D4B8", "#F1E2D7", "#F7D6E0",
                    "#BFD8B8", "#A8D0DB", "#D8F8F2", "#E9D7F7", "#A9D7C3", "#F5E4A0", "#F3D7B6",
                    "#FAD0C7", "#D4E157", "#D1F2A5", "#C1C6FF", "#FAD02E", "#F9C2C2", "#BBE6E0",
                    "#F1D3E0", "#D8C3C1", "#F2C4D0", "#FFB4D9", "#F5B7B1", "#C1E4F2", "#A1E7D7",
                    "#F7D2FF", "#F5C3E8", "#D7C8F4", "#E0D3F7", "#C6D1FF", "#B9D8C9", "#D4B2A1",
                    "#F2A6C1", "#A3D5F5", "#F2C3FF", "#F5C1A9", "#F7B5FF", "#F9D9D0", "#E3E6D9",
                    "#E9D1A4", "#FFB1D0", "#B9B3E0", "#E5C9F2", "#F9A5C9", "#F3E1B5", "#F1F0D1",
                    "#A3F4D6", "#F6DAF1", "#FF96B7", "#A0B9FF", "#D6F0D6", "#A8E6FF", "#F1A0D8",
                    "#FF80C4", "#E9C8A6", "#C6FF91", "#A1F4A5", "#D0B6FF", "#F1E5F9", "#F3A0A8",
                    "#F0E6D9", "#F7D1A4", "#F1B2C1", "#A6D6F4"
                ];
                break;
            case 'vibrant':
                // Canlı renk listesi
                colorList = [
                    "#FF6347", "#FF4500", "#FF0000", "#32CD32", "#FFFF00", "#00FF00", "#008080",
                    "#00CED1", "#0000FF", "#800080", "#FFD700", "#8B0000", "#800000", "#008000",
                    "#00008B", "#A52A2A", "#D2691E", "#A9A9A9", "#BDB76B", "#CD5C5C", "#20B2AA"
                ];
                break;
            default:
                // Varsayılan olarak pastel renkler seçilir
                colorList = [
                    "#FF6F61", "#FF7F50", "#FF7F32", "#FF9A8B", "#FFFF66", "#FFDF00"  // Varsayılan pastel renkler
                ];
                break;
        }

        /**
         * Hex kodunu RGB formatına dönüştüren yardımcı fonksiyon.
         * @param {string} hex - Hex renk kodu.
         * @returns {Object} RGB değerlerini içeren bir nesne.
         */
        const hexToRgb = (hex) => {
            const r = parseInt(hex.substr(1, 2), 16);  // Kırmızı bileşen
            const g = parseInt(hex.substr(3, 2), 16);  // Yeşil bileşen
            const b = parseInt(hex.substr(5, 2), 16);  // Mavi bileşen
            return { r, g, b };
        };

        /**
         * İki renk arasındaki mesafeyi hesaplar. (RGB değerlerine göre)
         * @param {string} color1 - İlk renk (hex kodu).
         * @param {string} color2 - İkinci renk (hex kodu).
         * @returns {number} İki renk arasındaki mesafe (daha küçük değerler daha yakın renkleri gösterir).
         */
        const colorDistance = (color1, color2) => {
            const rgb1 = hexToRgb(color1);
            const rgb2 = hexToRgb(color2);
            return Math.sqrt(Math.pow(rgb1.r - rgb2.r, 2) + Math.pow(rgb1.g - rgb2.g, 2) + Math.pow(rgb1.b - rgb2.b, 2));
        };

        // Başlangıçta ilk renk olarak ilk rengi kabul et
        let closestColor = colorList[0];
        let smallestDistance = colorDistance(selectedColor, closestColor);

        // Tüm renkler arasında en yakın rengi bul
        colorList.forEach(color => {
            const distance = colorDistance(selectedColor, color);
            if (distance < smallestDistance) {
                closestColor = color;
                smallestDistance = distance;
            }
        });

        // En yakın renk bulundu, input elemanının değerini bu renk ile değiştir
        document.getElementById(elementId).value = closestColor;

        // Kullanıcıyı bilgilendiren bir mesaj göster
        toastr.info(`Seçilen renk, kullanım uygunluğuna sadık kalma sebebiyle en yakın renk olan ${closestColor} ile değiştirildi.`, 'Renk Değiştirildi');
    }
    ,
    //DATE TIME PICKER formatter
    /**
     * Belirtilen  input elemanını tarih seçiciciye çevirecek şekilde formatlar.
     * 
     * @param {string} targetElement - formatlanacak elemanının id'si.
     */
    setDatePicker: function (targetId, type) {
        try {
            switch (type) {
                case "date":
                    $('#' + targetId).flatpickr({
                        enableTime: false,            // Saat seçimi aktif
                        dateFormat: 'd/m/Y',    // Tarih formatı
                    });
                    break;

                case "time":
                    $('#' + targetId).flatpickr({
                        enableTime: true,            // Saat seçimi aktif
                        dateFormat: 'H:i',    // Tarih formatı
                        noCalendar: true,
                        time_24hr: true,            // 24 saatlik format
                        /*locale: 'tr',*/               // Türkçe dil desteği
                        allowInput: true,
                    });
                    break;

                case "time-with-seconds":
                    $('#' + targetId).flatpickr({
                        enableTime: true,             // Saat seçimi aktif
                        enableSeconds: true,          // Saniye seçimi aktif
                        dateFormat: 'H:i:S',          // Saat:dakika:saniye formatı
                        noCalendar: true,
                        time_24hr: true,              // 24 saatlik format
                        /*locale: 'tr',*/                 // Türkçe dil desteği
                        allowInput: true,
                    });
                    break;

                default:
                    $('#' + targetId).flatpickr({
                        enableTime: true,            // Saat seçimi aktif
                        dateFormat: 'd/m/Y H:i',    // Tarih formatı
                        time_24hr: true,            // 24 saatlik format
                        /*locale: 'tr',*/               // Türkçe dil desteği
                    });
            }



        } catch (error) {
            console.error('Datetimepicker initialization error:', error);
        }
    }
    ,

    //FILL DROPDOWN
    /**
     * Gerekli verileri alarak dropdown listesini doldurur.
     */
    fillDropdowns: function () {
        $(".zuraf-select").each(function () {
            var dropdown = $(this);
            var targetUrl = dropdown.data("fill-target-url");
            // Placeholder değerini al
            var placeholder = dropdown.attr("placeholder");

            // AJAX isteği göndererek verileri al
            $.ajax({
                url: targetUrl,
                type: "GET",
                dataType: "json",
                success: function (data) {
                    // Verileri alındıktan sonra dropdown'u doldur
                    dropdown.empty(); // Önce dropdown'u temizle

                    // Placeholder'ı ekleyin, eğer varsa
                    if (placeholder) {
                        dropdown.append($('<option disabled selected value="0"></option>').text(placeholder));
                    }

                    $.each(data, function (key, entry) {
                        dropdown.append($('<option></option>').attr('value', entry.value).text(entry.text));
                    });

                    // İlgili dropdown seçilen değeri yükle
                    dropdown.val(0);
                },
                error: function (xhr, status, error) {
                    console.error(xhr.responseText);
                    // Hata durumunda bir işlem yapılabilir
                }
            });
        });
    }
    ,
    //MASK
    /**
     * Belirtilen  input elemanına belirtilen türde mask ekler
     *
     *  
     * * maskType seçenekleri:
         * - phone-tr: Türkiye için  xxx xx xx xx formatlaması.
     * 
     * @param {string} targetElement - formatlanacak elemanının id'si.
     * @param {string} maskType - mask türü.
     */
    maskInput: function (targetElement, maskType) {


        var maskValue = "";
        var maskOptions = {};

        switch (maskType) {
            case 'phone-tr':
                maskValue = "999 999 99 99";
                break;

            case 'boy': // Boy için maske (0.50 - 2.50 metre)
                maskOptions = {
                    alias: "decimal",
                    digits: 2,
                    radixPoint: ".",
                    min: 0.50,
                    max: 2.50,
                    enforceDigitsOnBlur: true,
                    allowMinus: false,
                    suffix: " metre"
                };
                break;

            case 'nationalid-tr': // TC Kimlik Numarası için maske (11 haneli)
                maskValue = "99999999999";
                break;

            case 'kilo': // Kilo için maske (30 - 200 kg)
                maskOptions = {
                    alias: "integer",
                    min: 30,
                    max: 200,
                    allowMinus: false,
                    suffix: " kilogram"
                };
                break;

            default:
                break;
        }

        if (maskValue) {
            $(`#` + targetElement).inputmask(maskValue);
        } else if (Object.keys(maskOptions).length > 0) {
            $(`#` + targetElement).inputmask(maskOptions);
        }

    }
    ,
    //ADD VALIDATOUN ERROR MESSAGE
    /**
     * Belirtilen  input elemanına belirtilen hata mesajını ekler
     *
     * 
     * 
     * @param {string} targetElement -  elemanının id'si.
     * @param {string} message - mesaj.
     */
    addValidationErrorMessage: function (targetElement, message) {

        $('#' + targetElement + '-text-danger').remove();
        $('#' + targetElement).after('<div id="' + targetElement + '-text-danger" class="text-danger">' + message + '</div>');
        return;

    },
    // Modal içeriklerini dinamik olarak açma ve kapama işlevi
    /**
     * Kullanıcı profil fotoğrafını düzenlemek için modal açar.
     *
     * @param {HTMLElement} inputElement - Dosya seçme input elemanı.
     * @param {string} previewImgId - Önizleme resminin ID'si.
     * @param {string} hiddenFileInputId - Kırpılmış dosyanın gizli input ID'si.
     */
    openCropModal: function (inputElement, previewImgId, hiddenFileInputId) {
        const file = inputElement.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function (e) {
                // Modal içeriğini dinamik olarak oluştur
                const modalContainer = document.getElementById('cropperModalContainer');
                modalContainer.innerHTML = `
                    <div class="modal modal-lg fade show" id="cropperModal" tabindex="-1" role="dialog" aria-labelledby="cropperModalLabel">
                        <div class="modal-dialog modal-dialog-centered" role="document">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" id="cropperModalLabel">Seçilen Görseli Düzenle</h5>
                                    <button class="btn-close" onclick="zuraf.closeModal('#cropperModal')" type="button" aria-label="Close"></button>
                                </div>
                                <div class="modal-body d-flex justify-content-center align-items-center">
                                    <div>
                                        <img id="image-to-crop" src="${e.target.result}" alt="Profile Image" style="max-width: 100%;" />
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <button onclick="zuraf.closeModal('#cropperModal')" class="btn btn-secondary btn-sm" type="button">Kapat</button>
                                    <button type="button" class="btn btn-primary" onclick="zuraf.cropImage()">Kırp</button>
                                </div>
                            </div>
                        </div>
                    </div>
                `;

                // Yeni cropper'ı başlat
                const image = document.getElementById('image-to-crop');
                $('#cropperModal').modal('show'); // Modal'ı aç
                zuraf.initCropper(image);
            };
            reader.readAsDataURL(file);
        }
    },

    /**
     * Resim kırpma işlemi için cropper başlatır.
     *
     * @param {HTMLElement} image - Kırpılacak resmin DOM elemanı.
     */
    initCropper: function (image) {
        cropper = new Cropper(image, {
            aspectRatio: 1, // Kare yapmak için
            minContainerWidth: 500, // Minimum genişlik
            minContainerHeight: 500, // Minimum yükseklik
            viewMode: 1,
            preview: '.preview',
            responsive: true,
            ready: function () {
                const cropperImage = this.cropper;
                const modalWidth = document.querySelector('.modal-dialog').offsetWidth;
                cropperImage.setCropBoxData({ width: modalWidth * 0.8 });  // Crop box'ı modal genişliğinin %80'ine ayarla
            }
        });
    },

    cropImage: function () {
        const canvas = cropper.getCroppedCanvas();
        const previewImage = document.getElementById('preview-img');
        previewImage.src = canvas.toDataURL();

        // Blob formatında resim oluştur
        const file = zuraf.dataURItoBlob(canvas.toDataURL());

        //// FileList formatında dosya oluşturulması
        //const fileList = new DataTransfer();
        //fileList.items.add(file);

        //// Formdaki dosya input'una atama
        //const resizedImageInput = document.getElementById('cropped-file');
        //resizedImageInput.val = fileList.files;

        // Base64 formatında veri al
        const base64Data = canvas.toDataURL();

        // Base64'ü bir hidden inputa koy
        const resizedImageInput = document.getElementById('cropped-file');
        resizedImageInput.value = base64Data; // "value" olarak atanmalı, "val" yanlış

        zuraf.closeModal('#cropperModal');
    }
    ,

    /**
     * Base64 görüntüsünü Blob'a dönüştürür.
     *
     * @param {string} dataURI - Base64 kodlu görüntü.
     * @returns {File} - Blob formatında görüntü dosyası.
     */
    dataURItoBlob: function (dataURI) {
        const byteString = atob(dataURI.split(',')[1]);
        const arrayBuffer = new ArrayBuffer(byteString.length);
        const intArray = new Uint8Array(arrayBuffer);

        for (let i = 0; i < byteString.length; i++) {
            intArray[i] = byteString.charCodeAt(i);
        }

        // Blob oluştur
        const blob = new Blob([intArray], { type: 'image/jpeg' });
        const file = new File([blob], 'profile.jpg', { type: 'image/jpeg' });

        return file;
    },
    initWizard: function() {
        var wizards = document.querySelectorAll('.theme-wizard');
        var tabPillEl = document.querySelectorAll('#pill-tab2 [data-bs-toggle="pill"]');
        var tabProgressBar = document.querySelector('.theme-wizard .progress');

        wizards.forEach(function (wizard) {
            var tabToggleButtonEl = wizard.querySelectorAll('[data-wizard-step]');
            var form = wizard.querySelector('[novalidate]');
            var nextButton = wizard.querySelector('.next button');
            var prevButton = wizard.querySelector('.previous button');
            var cardFooter = wizard.querySelector('.theme-wizard .card-footer');
            var count = 0;

            // Sayfa doğrulama fonksiyonu
            var validateForm = function () {
                var isValid = true;
                var inputs = wizard.querySelectorAll('input, select, textarea'); // Tüm inputları al
                inputs.forEach(function (input) {
                    if (!input.checkValidity()) {
                        isValid = false;
                    }
                });
                return isValid;
            };

            prevButton.classList.add('d-none'); // İlk başta prev butonunu gizle

            // Next button click
            nextButton.addEventListener('click', function () {
                if (!validateForm() && form.className.includes('needs-validation')) {
                    form.classList.add('was-validated'); // Validasyon hata durumu
                } else {
                    count += 1;
                    var tab = new window.bootstrap.Tab(tabToggleButtonEl[count]);
                    tab.show();
                }
            });

            // Prev button click
            prevButton.addEventListener('click', function () {
                count -= 1;
                var tab = new window.bootstrap.Tab(tabToggleButtonEl[count]);
                tab.show();
            });

            tabToggleButtonEl.forEach(function (item, index) {
                item.addEventListener('shown.bs.tab', function (e) {
                    if (!validateForm() && form.className.includes('needs-validation')) {
                        e.preventDefault();
                        form.classList.add('was-validated');
                        return null;
                    }

                    count = index; // Geçerli sekma indeksi

                    // Önce tüm sekmaları tamamlandı olarak işaretle
                    for (var i = 0; i < count; i++) {
                        tabToggleButtonEl[i].classList.add('done');
                    }
                    // Kalanlar için done sınıfını kaldır
                    for (var j = count; j < tabToggleButtonEl.length; j++) {
                        tabToggleButtonEl[j].classList.remove('done');
                    }

                    // Butonların görünürlüğünü kontrol et
                    if (count === tabToggleButtonEl.length - 1) {
                        // Son sekmada: Next gizli, Gönder görünür
                        nextButton.classList.add('d-none');
                        saveButton.classList.remove('d-none');
                    } else {
                        // Diğer sekmalarda: Next görünür, Gönder gizli
                        nextButton.classList.remove('d-none');
                        saveButton.classList.add('d-none');
                    }

                    // Previous butonu: ilk sekma hariç göster
                    if (count > 0) {
                        prevButton.classList.remove('d-none');
                    } else {
                        prevButton.classList.add('d-none');
                    }

                    // Footer'ın tamamını gizlemeyin!
                    // cardFooter.classList.remove('d-none'); gibi footer hep görünür kalmalı.
                });
            });

        });

        // Control wizard progressbar
        if (tabPillEl.length) {
            var dividedProgressbar = 100 / tabPillEl.length;
            tabProgressBar.querySelector('.progress-bar').style.width = "".concat(dividedProgressbar, "%");
            tabPillEl.forEach(function (item, index) {
                item.addEventListener('shown.bs.tab', function () {
                    tabProgressBar.querySelector('.progress-bar').style.width = "".concat(dividedProgressbar * (index + 1), "%");
                });
            });
        }
    }
};
