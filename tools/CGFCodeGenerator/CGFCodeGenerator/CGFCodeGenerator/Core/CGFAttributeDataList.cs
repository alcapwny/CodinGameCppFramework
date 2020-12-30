using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CGFCodeGenerator.Core
{
    public class CGFAttributeDataList
    {
        public static CGFAttributeDataList Parse(CGFParserReporter reporter, ImmutableArray<Microsoft.CodeAnalysis.AttributeData> attributes)
        {
            List<CGFAttributeData> attributeDataList = new List<CGFAttributeData>();
            foreach (Microsoft.CodeAnalysis.AttributeData attributeData in attributes)
            {
                CGFAttributeData CGFAttributeData = CGFAttributeData.Parse(reporter, attributeData);
                if (CGFAttributeData != null)
                {
                    attributeDataList.Add(CGFAttributeData);
                }
            }

            CGFAttributeDataList CGFAttributeDataList = new CGFAttributeDataList(attributeDataList);

            return CGFAttributeDataList;
        }

        CGFAttributeDataList(List<CGFAttributeData> attributes)
        {
            m_Attributes = attributes;
        }

        public T GetAttribute<T>() where T : Attribute
        {
            CGFAttributeData attribute = m_Attributes.Find(attributeData => (attributeData.Attribute as T != null));
            if (attribute == null)
                return null;

            return attribute.Attribute as T;
        }

        List<CGFAttributeData> m_Attributes;
    }
}
