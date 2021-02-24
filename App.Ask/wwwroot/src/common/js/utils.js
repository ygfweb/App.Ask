/** ajax帮助类 */
class AjaxHelper {
    /**
     * post表单数据
     * @param {string} url
     * @param {FormData} formData
     */
    static postFormData(url, formData) {
        return $.ajax({
            url: url,
            data: formData,
            method: "POST",
            cache: false,
            processData: false,
            contentType: false,
        });
    }

    /**
     * 提交表单
     * @param {string} url
     * @param {HTMLElement} formDom
     */
    static postForm(url, formDom) {
        let data = new FormData(formDom);
        return this.postFormData(url, data);
    }

    /**
     * 提交JSON数据
     * @param {string} url
     * @param {object} jsonObject
     */
    static postJson(url, jsonObject) {
        return $.ajax({
            url: url,
            data: JSON.stringify(jsonObject),
            method: "POST",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            processData: true,
            cache: false,
        })
    }

    /**
    * 显示表单错误信息
    * @param {HTMLElement} formDom
    * @param {object} ajaxResult
    */
    static showFormError(formDom, ajaxResult) {
        let v = $(formDom).validate();
        v.showErrors(ajaxResult.ErrorMessages);
        if (ajaxResult.Message) {
            DialogHelper.showErrorDialog(ajaxResult.Message);
        }
    }
}

class DialogHelper {
    /**
     * 显示消息对话框
     * @param {string} msg
     * @param {Function} okFunc
     */
    static showMessageDialog(msg, okFunc) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_PRIMARY,
            title: '提示',
            message: msg,
            buttons: [{
                label: '确定',
                action: function (dialogRef) {
                    dialogRef.close();
                    if (okFunc) {
                        okFunc();
                    }
                }
            }]
        });
    }

    /**
     * 显示确认对话框
     * @param {any} msg
     * @param {any} okFunc
     */
    static showConfirmDialog(msg, okFunc) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_PRIMARY,
            title: '提示',
            message: msg,
            buttons: [{
                label: '确定',
                cssClass: 'btn-primary',
                action: function (dialogRef) {
                    dialogRef.close();
                    okFunc();
                }
            }, {
                label: '取消',
                action: function (dialogRef) {
                    dialogRef.close();
                }
            }]
        });
    }

    static showErrorDialog(msg, okFunc) {
        BootstrapDialog.show({
            type: BootstrapDialog.TYPE_DANGER,
            title: '提示',
            message: msg,
            buttons: [{
                label: '确定',
                action: function (dialogRef) {
                    dialogRef.close();
                    if (okFunc) {
                        okFunc();
                    }
                }
            }]
        });
    }

    static showSuccess(msg, okFunc, buttonText) {
        if (!buttonText) {
            buttonText = "确定";
        }
        Swal.fire({
            title: msg,
            icon: 'success',
            allowOutsideClick: false,
            allowEscapeKey: false,
            showCancelButton: false,
            confirmButtonText: buttonText
        }).then((result) => {
            if (result.isConfirmed) {
                if (okFunc) {
                    okFunc();
                }
            }
        })
    }

    static showToastError(errorMsg) {
        $.toast({
            heading: '错误',
            position: 'bottom-right',
            text: errorMsg,
            showHideTransition: 'fade',
            icon: 'error'
        })
    }

    static showToastSuccess(msg) {
        $.toast({
            heading: '提示',
            position: 'bottom-right',
            text: msg,
            showHideTransition: 'fade',
            icon: 'success'
        })
    }

    static showSpinner() {
        let spinner = document.getElementById("spinner-wrapper");
        if (spinner) {
            spinner.classList.remove("d-none");
        }
    }

    static closeSpinner() {
        let spinner = document.getElementById("spinner-wrapper");
        if (spinner) {
            spinner.classList.add("d-none");
        }
    }
}

class DomHelper {
    /**
     * 设置tags输入框
     * @param {HTMLElement} inputDom
     * @param {Array} typeaheadSource
     */
    static setTagsinput(domSelector, typeaheadSource) {
        var data = typeaheadSource;
        var states = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.whitespace,
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            local: data
        });
        states.initialize();
        $(domSelector).tagsinput({
            allowDuplicates: false,
            tagClass: "badge badge-info p-1",
            confirmKeys: [13],
            maxTags: 5,
            typeaheadjs: {
                name: "states",
                source: states.ttAdapter(),
                afterSelect: function (val) { this.$element.val(""); },
            },
            freeInput: true
        });
        function isTagText(tagName) {
            var regu = "^[a-zA-Z0-9\u4e00-\u9fa5\\s*]+$";
            var re = new RegExp(regu);
            return re.test(tagName);
        }
        $(".tt-input").on("blur", function () {
            $(this).val("");
        })
        $('.bootstrap-tagsinput').focusin(function () {
            $(this).addClass('focus');
        });
        $('.bootstrap-tagsinput').focusout(function () {
            $(this).removeClass('focus');
        });
        $('#tags').on('beforeItemAdd', function (event) {
            if (!isTagText(event.item)) {
                event.cancel = true;
            } else {
                event.cancel = false;
            }
        });
    }

    static setTinymce(domSelector, min_height) {
        tinymce.init({
            selector: domSelector,
            language: 'zh_CN',
            contextmenu: false, // 禁止编辑器右键菜单
            branding: false, //去掉版权
            convert_urls: false,  // 禁止自动转换路径，默认会将一个绝对路径转换成相对路径
            plugins: [
                'link autolink image lists advlist code table paste codesample fullscreen template hr media noneditable paste preview searchreplace'
            ],
            valid_elements: '*[*]',
            toolbar: [
                ' bold italic underline | link image media | bullist numlist | removeformat preview fullscreen',
            ],
            custom_colors: false,
            force_br_newlines: true,
            force_p_newlines: false,
            min_height: min_height,
            menubar: false,
            paste_block_drop: true,
            paste_enable_default_filters: true,
            a_plugin_option: true,
            default_link_target: '_blank',
            link_context_toolbar: true,
            a_configuration_option: 400,
            formats: {
                mark: { inline: 'mark' }, // 高亮文本
                italic: { inline: 'em' }  // 斜体
            },
            style_formats: [
                {
                    title: 'Inline', items: [
                        { title: 'Bold', format: 'bold' },  // 粗体
                        { title: 'Italic', format: 'italic' }, // 斜体
                        { title: 'Strikethrough', format: 'strikethrough' },
                        { title: '高亮文本', format: 'mark' }
                    ]
                },
                {
                    title: 'Blocks', items: [
                        { title: 'Paragraph', format: 'p' },
                        { title: 'Blockquote', format: 'blockquote' }
                    ]
                }
            ],
            image_caption: false,
            image_advtab: false,
            image_dimensions: false,
            image_title: true,
            image_description: false,
            images_upload_handler: function (blobInfo, success, failure) {
                let formData = new FormData();
                formData.append('file', blobInfo.blob(), blobInfo.filename());
                $.ajax({
                    url: "/Upload",
                    data: formData,
                    method: "POST",
                    cache: false,
                    processData: false,
                    contentType: false,
                }).done(function (result) {
                    if (result.Success) {
                        let imgUrl = result.Context;
                        success(imgUrl);
                    } else {
                        failure(result.Message);
                    }
                }).fail(function (jqXHR, status, err) {
                    if (jqXHR.status === 0) {
                        failure("浏览器的网络出现故障")
                    } else if (jqXHR.status == 404) {
                        failure('服务器无法根据客户端的请求找到资源. [404]');
                    } else if (jqXHR.status == 500) {
                        failure('服务器内部错误 [500].');
                    } else if (jqXHR.status == 401) {
                        failure("请求要求用户的身份认证.[401]")
                    } else if (exception === 'timeout') {
                        failure('请求超时.');
                    } else if (exception === 'abort') {
                        failure('请求被中断.');
                    } else {
                        failure('未捕获的错误.\n' + jqXHR.responseText);
                    }
                });
            },
            table_advtab: false,
            table_cell_advtab: false,
            table_row_advtab: false,
            table_appearance_options: false,
            table_grid: true,
            table_resize_bars: false,
            table_default_styles: {},
            table_class_list: [
                { title: '默认', value: '' },
                { title: '简洁', value: 'table' },
                { title: '条纹行', value: 'table table-striped' },
                { title: "边框线", value: 'table table-bordered' }
            ],
            content_css: ["/src/site/common/css/theme.css", "/src/site/common/css/editor.css", "/assets/font-awesome/css/font-awesome.css"],
        });
    }
    static setSummernote(domSelector) {
        $(domSelector).summernote({
            placeholder: 'Hello stand alone ui',
            tabsize: 2,
            minHeight: 350,
            lang: 'zh-CN',
            dialogsFade: true,
            popover: {
                image: [
                    ['custom', ['bs4ImageLeft', 'bs4ImageCenter', 'bs4ImageRight', 'bs4ImageReset']],
                    ['remove', ['removeMedia']]
                ],
                link: [
                    ['link', ['linkDialogShow', 'unlink']]
                ],
                table: [
                    ['add', ['addRowDown', 'addRowUp', 'addColLeft', 'addColRight']],
                    ['delete', ['deleteRow', 'deleteCol', 'deleteTable']],
                ],
                air: [
                    ['color', ['color']],
                    ['font', ['bold', 'underline', 'clear']],
                    ['para', ['ul', 'paragraph']],
                    ['table', ['table']],
                    ['insert', ['link', 'picture']]
                ]
            },
            toolbar: [
                ['Misc', ['undo', 'redo']],
                ['style', ['bold', 'italic', 'underline', 'clear']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['table', ['table']],
                ['insert', ['link', 'picture', 'video', 'hello']],
                ['view', ['fullscreen', 'codeview']]
            ],
            callbacks: {
                onImageUpload: function (files, editor) {
                    var $editor = $(this);
                    let data = new FormData();
                    data.append("file", files[0]);
                    AjaxHelper.postFormData("/Upload", data).done(function (result) {
                        if (result.Success) {
                            let imgUrl = result.Context;
                            let imgNode = document.createElement("img");
                            imgNode.setAttribute("src", imgUrl);
                            $editor.summernote('insertNode', imgNode);
                        } else {
                            DialogHelper.showErrorDialog(result.Message);
                        }
                    }).fail(function (jqXHR) {
                        if (jqXHR.status === 0) {
                            DialogHelper.showErrorDialog("浏览器的网络出现故障")
                        } else if (jqXHR.status == 404) {
                            DialogHelper.showErrorDialog('服务器无法根据客户端的请求找到资源. [404]');
                        } else if (jqXHR.status == 500) {
                            DialogHelper.showErrorDialog('服务器内部错误 [500].');
                        } else if (jqXHR.status == 401) {
                            DialogHelper.showErrorDialog("请求要求用户的身份认证.[401]")
                        } else if (exception === 'timeout') {
                            DialogHelper.showErrorDialog('请求超时.');
                        } else if (exception === 'abort') {
                            DialogHelper.showErrorDialog('请求被中断.');
                        } else {
                            DialogHelper.showErrorDialog('未捕获的错误.\n' + jqXHR.responseText);
                        }
                    })
                },
                onChange: function (contents, $editable) {
                    var imgs = $('.note-editable').find("img");
                    $.each(imgs, function (index, img) {
                        $(img).removeAttr("style");;
                        $(img).addClass("img-fluid");
                    })
                }
            }
        });
    }
}

$(function () {
    $('[data-toggle="popover"]').popover({
        container: 'body',
        trigger: 'hover',
        placement: "auto"
    });
    $(document).ajaxStart(function () {
        if (Pace) {
            Pace.restart();
        }
    });
})

function _setVideoSize(frameDom) {
    let w = $(frameDom).width();
    let h = w * 9 / 16;
    $(frameDom).height(h);
}

function _setAllVideoSize() {
    var $allVideos = $("iframe");
    $allVideos.each(function () {
        _setVideoSize(this);
    });
}

$(function () {
    _setAllVideoSize();
    $(window).resize(function () {
        _setAllVideoSize();
    }).resize();
})

// 禁止ios10以上缩放
window.addEventListener(
    "touchmove",
    function (event) {
        if (event.scale !== 1) {
            event.preventDefault();
        }
    },
    { passive: false }
);