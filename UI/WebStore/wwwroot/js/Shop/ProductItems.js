ProductItems = {
    _properties: {
        getUrl: ""
    },
    
    init: properties => {
        $.extend(ProductItems._properties, properties);
        $(".pagination li a").on("click", clickOnPage)
    },

    clickOnPage: function(e) {
        e.preventDefault()
        const button = $(this)
        
        if (button.prop("href").length > 0) {
            const page = button.data("page")
            const container = $("#items-container")
            container.LoadingOverlaySetup("show")
            const data = button.data()
            let query = ""
            for (let key in data){
                if(data.hasOwnProperty(key)){
                    query += `${key}=${data[key]}$`
                }
            }
            
            $.get(`${ProductItems._properties.getUrl}?${query}`)
                .done(result => {
                    container.html(result)
                    container.LoadingOverlay("hide")
                    
                    $(".pagination li").removeClass("active")
                    $(".pagination li a").prop("href","#")
                    $(`.pagination li a[data-page=${page}]`)
                        .removeAttr("href")
                        .parent().addClass("active")
                })
                .fail(() => console.log("clickOnPage getItems error"))
        }
    }
}