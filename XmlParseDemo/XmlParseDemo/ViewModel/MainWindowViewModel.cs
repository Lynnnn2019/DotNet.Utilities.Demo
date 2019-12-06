using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlParseDemo.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region 属性
        // Item的树形结构
        private ObservableCollection<ItemTreeData> itemTreeDataList;
        public ObservableCollection<ItemTreeData> ItemTreeDataList
        {
            get
            {
                return itemTreeDataList;
            }
            set
            {
                itemTreeDataList = value;
                RaisePropertyChanged("ItemTreeDataList");
            }
        }
        // 属性值
        private ObservableCollection<PropertyData> propertyDatas;
        public ObservableCollection<PropertyData> PropertyDatas
        {
            get
            {
                return propertyDatas;
            }
            set
            {
                propertyDatas = value;
                RaisePropertyChanged("PropertyDatas");
            }
        }
        private const string xmlPath = @"test.xml";
        #endregion

        #region 构造函数
        public MainWindowViewModel()
        {
            ItemTreeDataList = new ObservableCollection<ItemTreeData>();
        }
        #endregion

        #region 方法
        int index = 0;//用于节点递归
        /// <summary>
        /// 读取xml文件信息
        /// </summary>
        public void ReadXml()
        {
            try
            {
                #region 第一种方式、包含未知属性
                //XmlReader xr = XmlReader.Create(xmlPath);
                //while (xr.Read())
                //{
                //    if (xr.HasAttributes)
                //    {
                //        Console.WriteLine("<" + xr.Name + ">的属性：");
                //        for (int i = 0; i < xr.AttributeCount; i++)
                //        {
                //            xr.MoveToAttribute(i);              //Console.WriteLine("<" + xr.Name + ">的属性：");
                //            Debug.WriteLine("{0}={1}", xr.Name, xr.Value);
                //        }
                //    }
                //}
                #endregion

                #region 第二种方式、LINQ TO XML

                //1、加载Xml文件
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlPath);

                //2、获取根元素
                var rootNode = xmldoc.DocumentElement;
                //3、将根元素加载到TreeView的根节点上
                ItemTreeData rootTreeNode = new ItemTreeData()
                {
                    itemId = index++,
                    itemName = rootNode.Name,
                    IsExpanded = true,
                    itemParent = 0,
                    itemStep = 0,
                    Propertys = new ObservableCollection<PropertyData>()
                };
                foreach (var item in rootNode.Attributes)
                {
                    XmlAttribute xmlAttribute = item as XmlAttribute;
                    rootTreeNode.Propertys.Add(
                        new PropertyData()
                        {
                            Name = xmlAttribute.Name,
                            Value = xmlAttribute.Value
                        });
                }
                //4、调用递归方法
                LoadXmlToTreeView(rootNode, rootTreeNode, 0);
                Debug.WriteLine("{0}加载完成", rootTreeNode.itemName);
                ItemTreeDataList.Add(rootTreeNode);
                #endregion

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString(), "错误信息");
            }
        }

        /// <summary>
        /// 递归方法
        /// </summary>
        /// <param name="xmlrootele"></param>
        /// <param name="nodes"></param>
        private void LoadXmlToTreeView(XmlElement xmlrootele, ItemTreeData itemTreeData, int step)
        {
            //循环根元素的子元素

            foreach (XmlNode item in xmlrootele.ChildNodes)
            {

                //需要判断当前节点是什么类型
                if (item.NodeType == XmlNodeType.Element)
                {
                    XmlElement xmlElement = (XmlElement)item;
                    ItemTreeData treeNode = new ItemTreeData()
                    {
                        itemId = index++,
                        itemStep = step + 1,
                        itemParent = itemTreeData.itemId,
                        itemName = xmlElement.Name,
                        IsExpanded = true,
                    };
                    foreach (var node in item.Attributes)
                    {
                        XmlAttribute xmlAttribute = node as XmlAttribute;
                        treeNode.Propertys.Add(
                            new PropertyData()
                            {
                                Name = xmlAttribute.Name,
                                Value = xmlAttribute.Value
                            });
                    }
                    LoadXmlToTreeView(xmlElement, treeNode, step + 1);
                    itemTreeData.Children.Add(treeNode);
                }
                else if (item.NodeType == XmlNodeType.Text || item.NodeType == XmlNodeType.CDATA)
                {
                    //nodes.Add(item.InnerText);
                }
            }
        }
        /// <summary>
        /// 设置xml文件信息
        /// </summary>
        public void WriteXml()
        {
            //修改测试20191206--
        }

        private bool hasFindSelectItem = false;
        /// <summary>
        /// 树形列表更改
        /// </summary>
        public void SelectItemChange(int indexId)
        {
            foreach (var item in itemTreeDataList)
            {
                if (item.itemId == indexId)
                {
                    PropertyDatas = item.Propertys;
                    hasFindSelectItem = true;
                    return;
                }
                else
                {
                    GetItemTreeData(item, indexId);
                }
            }
            if (hasFindSelectItem)
            {
                Debug.WriteLine("找到对应项");
                hasFindSelectItem = false;
            }
            
        }

        /// <summary>
        /// 获取指定的列表项
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private void GetItemTreeData(ItemTreeData itemTreeData, int indexId)
        {
            if (hasFindSelectItem)
            {
                return;
            }
            foreach (var item in itemTreeData.Children)
            {
                if (item.itemId == indexId)
                {
                    PropertyDatas = item.Propertys;
                    return;
                }
                else
                {
                    GetItemTreeData(item, indexId);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 自定义Item的树形结构
    /// </summary>
    public class ItemTreeData : BaseViewModel
    {
        public int itemId { get; set; }      // ID
        public string itemName { get; set; } // 名称
        public int itemStep { get; set; }    // 所属的层级
        public int itemParent { get; set; }  // 父级的ID

        private ObservableCollection<ItemTreeData> _children = new ObservableCollection<ItemTreeData>();
        public ObservableCollection<ItemTreeData> Children
        {  // 树形结构的下一级列表
            get
            {
                return _children;
            }
        }

        /// <summary>
        /// 属性集合
        /// </summary>
        private ObservableCollection<PropertyData> propertys = new ObservableCollection<PropertyData>();
        public ObservableCollection<PropertyData> Propertys
        {  // 树形结构的下一级列表
            get
            {
                return propertys;
            }
            set
            {
                propertys = value;
                RaisePropertyChanged("Propertys");
            }
        }
        public bool IsExpanded { get; set; } // 节点是否展开
        public bool IsSelected { get; set; } // 节点是否选中
    }

    /// <summary>
    /// 自定义的属性结构
    /// </summary>
    public class PropertyData
    {
        public string Name { get; set; }//属性名称
        public string Value { get; set; }//属性值，暂定dbl类型
    }
}
