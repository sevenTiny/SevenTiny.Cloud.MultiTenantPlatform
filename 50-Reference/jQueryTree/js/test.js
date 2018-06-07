$(function () {

    var json =
        [{
            "name": "||",
            "id":"101",
            "icon": "icon-th",
            "nodeType":"连接符/表达式",
            "member":"Age",
            "condition":">(枚举大于)",
            "value":"?",
            "valueType":"?(?表示参数)",
            "child": [
                {
                    "name": "&&",
                    "icon": "icon-minus-sign",
                    "id":"103",
                    "parentId": "101",
                    "child": [
                        {
                            "name": "Age > 0",
                            "id":"105",
                            "icon": "",
                            "parentId": "103",
                            "child": []
                        },
                        {
                            "name": "Age < 10",
                            "id":"104",
                            "icon": "",
                            "parentId": "103",
                            "child": []
                        }
                    ]
                },
                {
                    "name": "DataTime > 20100001",
                    "id":"102",
                    "icon": "",
                    "parentId": "101",
                    "child": []
                }
            ]
        }];


    function tree(data) {
        for (var i = 0; i < data.length; i++) {
            var data2 = data[i];
            if (data[i].icon == "icon-th") {
                $("#rootUL").append("<li data-name='" + data[i].id + "'><span><i class=''></i> " + data[i].name + "</span></li>");
            } else {
                var children = $("li[data-name='" + data[i].parentId + "']").children("ul");
                if (children.length == 0) {
                    $("li[data-name='" + data[i].parentId + "']").append("<ul></ul>")
                }
                $("li[data-name='" + data[i].parentId + "'] > ul").append(
                    "<li data-name='" + data[i].id + "'>" +
                    "<span>" +
                    "<i class=''></i> " +
                    data[i].name +
                    "</span>" +
                    "</li>")
            }
            for (var j = 0; j < data[i].child.length; j++) {
                var child = data[i].child[j];
                var children = $("li[data-name='" + child.parentId + "']").children("ul");
                if (children.length == 0) {
                    $("li[data-name='" + child.parentId + "']").append("<ul></ul>")
                }
                $("li[data-name='" + child.parentId + "'] > ul").append(
                    "<li data-name='" + child.id + "'>" +
                    "<span>" +
                    "<i class=''></i> " +
                    child.name +
                    "</span>" +
                    "</li>")
                var child2 = data[i].child[j].child;
                tree(child2)
            }
            tree(data[i]);
        }

    }

    tree(json)


});